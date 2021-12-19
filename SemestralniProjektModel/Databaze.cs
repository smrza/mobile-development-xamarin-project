using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SemestralniProjektModel
{

    //Pro tuto verzi byl využit NuGet package Microsoft.Data.SQLite
    //Práce s databází viz:
    //https://www.nuget.org/packages/Microsoft.Data.Sqlite/
    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite?view=msdata-sqlite-3.0.0
    //Oproti System.Data.SQLite.Core je v názvech tříd potřeba změnit SQL... na Sql...
    //Navíc, třída SqliteConnection neobsahuje metodu CreateFile ... ale když se podíváme na kód této metody, tak to není taková hrůza, tak si vytvoříme vlastní verzi (viz https://github.com/OpenDataSpace/System.Data.SQLite/blob/master/System.Data.SQLite/SqliteConnection.cs)
    //Oproti sqlite-net-pcl nemá asynchronní volání, takže se během práce s databází zaškubne UI (v původní verzi z OPN to ale funguje stejně)
    //Microsoftem je doporučený NuGet package sqlite-net-pcl
    //Tato verze je pro ty, co nechtějí přepisovat kód databáze (čtěte: pro líné studenty ... určitě víte které myslím :-))

    public class Databaze
    {

        private readonly string databazeFileName;
        private readonly string connectionString;
        public long LoggedIdUzivatele;


        public Databaze(string databazeFileName)
        {
            this.databazeFileName = databazeFileName;
            connectionString = $"Data Source={databazeFileName};";    //tady bylo ještě "Version=3;", ale aktuální NuGet volbu verze nepodporuje (v současnosti by měla být vždy automaticky použita verze 3)
        }


        public bool JeDatabazeVytvorena()
        {
            if (System.IO.File.Exists(databazeFileName))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// vytvori soubor - stejna metoda jako v System.Data.Sqlite.Core
        /// </summary>
        /// <param name="databazeFileName"></param>
        public static void CreateFile(string databazeFileName)
        {
            using (FileStream fs = File.Create(databazeFileName))
            {
                fs.Close();
            }
        }


        public void VytvorDatabazi()
        {
            if (JeDatabazeVytvorena() == false)
            {
                Databaze.CreateFile(databazeFileName);  //zde vytváříme soubor s databázi pomocí naší metody a ne té původní
            }

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string commandText = $"CREATE TABLE IF NOT EXISTS {Produkt.TableName} ({Produkt.IDString} integer primary key, {Produkt.NazevString} varchar(20) not null, {Produkt.KategorieString} varchar(20) not null, {Produkt.CenaString} int not null, {Produkt.PopisString} varchar(500) not null, {Produkt.BarvaString} varchar(30) not null)";
                string commandText2 = $"CREATE TABLE IF NOT EXISTS {Uzivatel.TableName} ({Uzivatel.IDString} integer primary key, {Uzivatel.JmenoString} varchar(20) not null)";
                string commandText3 = $"CREATE TABLE IF NOT EXISTS {Objednavka.TableName} ({Objednavka.IDString} integer primary key, {Objednavka.IDUzivatelString} integer, {Objednavka.StavString} varchar(20) not null, {Objednavka.DatumString} datetime not null, FOREIGN KEY({Objednavka.IDUzivatelString}) REFERENCES {Uzivatel.TableName}({Uzivatel.IDString}))";
                string commandText4 = $"CREATE TABLE IF NOT EXISTS {Kosik.TableName} ({Kosik.IDProduktString} integer, {Kosik.IDObjednavkaString} integer, FOREIGN KEY({Kosik.IDProduktString}) REFERENCES {Produkt.TableName}({Produkt.IDString}), FOREIGN KEY({Kosik.IDObjednavkaString}) REFERENCES {Objednavka.TableName}({Objednavka.IDString}))";
                SqliteCommand command = new SqliteCommand(commandText, connection);
                SqliteCommand command2 = new SqliteCommand(commandText2, connection);
                SqliteCommand command3 = new SqliteCommand(commandText3, connection);
                SqliteCommand command4 = new SqliteCommand(commandText4, connection);
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();
                command4.ExecuteNonQuery();

                connection.Close();
            }
        }


        public long VlozNeboUpdatujProdukt(Produkt produkt)
        {
            if (produkt.Id == 0)
            {
                return this.VlozProdukt(produkt.Nazev, produkt.Kategorie, produkt.Cena, produkt.Popis, produkt.Barva);
            }
            else
            {
                this.UpdatujProdukt(produkt.Id, produkt.Nazev, produkt.Kategorie, produkt.Cena, produkt.Popis, produkt.Barva);
                return 0;
            }
        }


        public long VlozProdukt(string nazev, string kategorie, int cena, string popis, string barva)
        {
            long idVlozenehoProduktu = 0;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                //po přidání je ještě zavolán příkaz: "SELECT last_insert_rowid();" pro navrácení posledního vloženého ID
                string commandText = $"INSERT INTO {Produkt.TableName} ({Produkt.NazevString}, {Produkt.KategorieString}, {Produkt.CenaString}, {Produkt.PopisString}, {Produkt.BarvaString}) values(@{Produkt.NazevString}, @{Produkt.KategorieString}, @{Produkt.CenaString}, @{Produkt.PopisString}, @{Produkt.BarvaString}); SELECT last_insert_rowid();";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Produkt.NazevString}", nazev);
                    command.Parameters.AddWithValue($"@{Produkt.KategorieString}", kategorie);
                    command.Parameters.AddWithValue($"@{Produkt.CenaString}", cena);
                    command.Parameters.AddWithValue($"@{Produkt.PopisString}", popis);
                    command.Parameters.AddWithValue($"@{Produkt.BarvaString}", barva);
                    idVlozenehoProduktu = (long)command.ExecuteScalar();
                }
                connection.Close();
            }
            return idVlozenehoProduktu;
        }


        public int UpdatujProdukt(long id, string nazev, string kategorie, int cena, string popis, string barva)
        {
            int pocetOvlivnenychRadku = 0;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"UPDATE {Produkt.TableName} SET {Produkt.NazevString} = @{Produkt.NazevString}, {Produkt.KategorieString} = @{Produkt.KategorieString}, {Produkt.CenaString} = @{Produkt.CenaString}, {Produkt.PopisString} = @{Produkt.PopisString}, {Produkt.BarvaString} = @{Produkt.BarvaString} WHERE {Produkt.IDString} = @{Produkt.IDString}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Produkt.IDString}", id);
                    command.Parameters.AddWithValue($"@{Produkt.NazevString}", nazev);
                    command.Parameters.AddWithValue($"@{Produkt.KategorieString}", kategorie);
                    command.Parameters.AddWithValue($"@{Produkt.CenaString}", cena);
                    command.Parameters.AddWithValue($"@{Produkt.PopisString}", popis);
                    command.Parameters.AddWithValue($"@{Produkt.BarvaString}", barva);
                    pocetOvlivnenychRadku = command.ExecuteNonQuery();
                }

                connection.Close();
            }
            return pocetOvlivnenychRadku;
        }


        public void SmazProdukt(long id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"DELETE FROM {Produkt.TableName} WHERE {Produkt.IDString} = @{Produkt.IDString}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Produkt.IDString}", id);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }


        public List<Produkt> VratVsechnyProdukty()
        {
            List<Produkt> produkty = new List<Produkt>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"SELECT * FROM {Produkt.TableName}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader[Produkt.IDString];
                            string nazev = (string)reader[Produkt.NazevString];
                            string kategorie = (string)reader[Produkt.KategorieString];
                            int cena = Convert.ToInt32(reader[Produkt.CenaString]);
                            string popis = (string)reader[Produkt.PopisString];
                            string barva = (string)reader[Produkt.BarvaString];

                            Produkt produkt = new Produkt(id, nazev, kategorie, cena, popis, barva);
                            produkty.Add(produkt);
                        }
                    }
                }

                connection.Close();
            }

            return produkty;
        }

        public long VlozUzivatele(Uzivatel uzivatel)
        {
            return this.UlozUzivatele(uzivatel.Jmeno);   
        }

        public long UlozUzivatele(string jmeno)
        {
            long idVlozenehoUzivatele = 0;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                //po přidání je ještě zavolán příkaz: "SELECT last_insert_rowid();" pro navrácení posledního vloženého ID
                string commandText = $"INSERT INTO {Uzivatel.TableName} ({Uzivatel.JmenoString}) values(@{Uzivatel.JmenoString}); SELECT last_insert_rowid();";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Uzivatel.JmenoString}", jmeno);
                    idVlozenehoUzivatele = (long)command.ExecuteScalar();
                }
                connection.Close();
            }
            return idVlozenehoUzivatele;
        }

        public List<Uzivatel> VratVsechnyUzivatele()
        {
            List<Uzivatel> uzivatele = new List<Uzivatel>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"SELECT * FROM {Uzivatel.TableName}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = (long)reader[Uzivatel.IDString];
                            string jmeno = (string)reader[Uzivatel.JmenoString];

                            Uzivatel uzivatel = new Uzivatel(id, jmeno);
                            uzivatele.Add(uzivatel);
                        }
                    }
                }

                connection.Close();
            }

            return uzivatele;
        }

        public long ReturnUzivatele(Uzivatel uzivatel)
        {
            return this.SelectUzivatele(uzivatel.Id);
        }

        public long SelectUzivatele(long id)
        {
            long idUzivatele = 0;
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                //po přidání je ještě zavolán příkaz: "SELECT last_insert_rowid();" pro navrácení posledního vloženého ID
                string commandText = $"SELECT {Uzivatel.IDString} FROM {Uzivatel.TableName} WHERE {Uzivatel.IDString}=@{Uzivatel.IDString}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Uzivatel.IDString}", id);
                    idUzivatele = (long)command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return idUzivatele;
        }

        public void VlozObjednavku(long idUzivatel, List<Produkt> produkty)
        {
            long idObjednavka;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string commandText = $"INSERT INTO {Objednavka.TableName} ({Objednavka.IDUzivatelString}, {Objednavka.StavString}, {Objednavka.DatumString}) values(@{Objednavka.IDUzivatelString}, @{Objednavka.StavString}, @{Objednavka.DatumString}); select last_insert_rowid();";

                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Objednavka.IDUzivatelString}", idUzivatel);
                    command.Parameters.AddWithValue($"@{Objednavka.StavString}", StavObjednavky.ZALOZENA.ToString());
                    command.Parameters.AddWithValue($"@{Objednavka.DatumString}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                    command.ExecuteNonQuery();

                    //command.CommandText = "select last_insert_rowid()";
                    idObjednavka = (long)command.ExecuteScalar();
                }
                connection.Close();
            }

            foreach (Produkt produkt in produkty)
            {
                Objednej(idObjednavka, produkt.Id);
            }
        }

        private void Objednej(long idObjednavka, long idProdukt)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"INSERT INTO {Kosik.TableName} ({Kosik.IDObjednavkaString}, {Kosik.IDProduktString}) values(@{Kosik.IDObjednavkaString}, @{Kosik.IDProduktString})";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue($"@{Kosik.IDObjednavkaString}", idObjednavka);
                    command.Parameters.AddWithValue($"@{Kosik.IDProduktString}", idProdukt);


                    command.ExecuteNonQuery();
                }

                connection.Close();

            }
        }

        public void UpravStavObjednavky(long idObjednavky, StavObjednavky stav)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"UPDATE {Objednavka.TableName} SET {Objednavka.StavString} = '{stav}' WHERE {Objednavka.IDString} = {idObjednavky};";

                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {

                    command.ExecuteNonQuery();
                }

                connection.Close();

            }
        }

        public List<Objednavka> VratObjednavkyAdmin()
        {
            List<Objednavka> objednavky = new List<Objednavka>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"SELECT * FROM {Objednavka.TableName}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {


                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var idObjednavky = reader[Objednavka.IDString];
                                var idUzivatel = reader[Objednavka.IDUzivatelString];
                                StavObjednavky stav = (StavObjednavky)Enum.Parse(typeof(StavObjednavky), (string)reader[Objednavka.StavString]);
                                DateTime datum = DateTime.Parse((string)reader[Objednavka.DatumString]);

                                List<Produkt> produkty = VratProduktyObjednavka((long)idObjednavky);

                                objednavky.Add(new Objednavka((long)idObjednavky, (long)idUzivatel, produkty, stav, datum));

                            }
                            catch (Exception e)
                            {

                                throw e;
                            }

                        }

                    }
                }

                connection.Close();
            }

            return objednavky;
        }

        public List<Objednavka> VratObjednavkyUzivatel(long idUzivatel)
        {
            List<Objednavka> objednavky = new List<Objednavka>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"SELECT * FROM {Objednavka.TableName} WHERE {Objednavka.IDUzivatelString}={idUzivatel}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            try
                            {
                                var id = reader[Objednavka.IDString];
                                var zakaznikId = reader[Objednavka.IDUzivatelString];
                                StavObjednavky stav = (StavObjednavky)Enum.Parse(typeof(StavObjednavky), (string)reader[Objednavka.StavString]);
                                DateTime datum = DateTime.Parse((string)reader[Objednavka.DatumString]);

                                List<Produkt> produkty = VratProduktyObjednavka((long)id);

                                objednavky.Add(new Objednavka((long)id, (long)zakaznikId, produkty, stav, datum));

                            }
                            catch (Exception e)
                            {

                                throw e;
                            }
                        }
                    }
                }

                connection.Close();
            }

            return objednavky;
        }

        public List<Produkt> VratProduktyObjednavka(long idObjednavka)
        {
            List<Produkt> produkty = new List<Produkt>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string commandText = $"SELECT * FROM {Kosik.TableName} JOIN {Produkt.TableName} ON {Kosik.IDProduktString}={Produkt.IDString} WHERE {Kosik.IDObjednavkaString}={idObjednavka}";
                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {

                                long id = (long)reader[Produkt.IDString];
                                string nazev = (string)reader[Produkt.NazevString];
                                string kategorie = (string)reader[Produkt.KategorieString];
                                object cena = reader[Produkt.CenaString];
                                string popis = (string)reader[Produkt.PopisString];
                                string barva = (string)reader[Produkt.BarvaString];

                                int price = Convert.ToInt32(cena);
                                Produkt produkt = new Produkt(id, nazev, kategorie, price, popis, barva);
                                produkty.Add(produkt);
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }

                        }
                    }
                }

                connection.Close();
            }

            return produkty;
        }
    }
}
