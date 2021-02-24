using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{

    //HİKAYE
    /*
     3 Veritabanı ile çalışan çok kullanıcılı bir sistem düşünelim. Bu veritabanları farklı kullanıcılar tarafından
     sürekli kullanılmakta. İşlemler 3 ayrı veritabanı ile gerçekleştiği için bir "Veritabanlari" class ı içerisine 
     çalışılan 3 veritabanı kodlarını ekledim. Hangi veritabanı ile işlem yapılacak ise üretilen ya da havuzdan çekilen
     nesne ile 3 veritabanına da ulaşılabilecek. Burada önemli olan her bir veritabanı işleminde veritabanlarına
     ulaşmak için "Veritabanlari" class ından bir nesne üretilmesi gerekiyor. Ancak nesne üretmek program için çok 
     pahalı bir iştir. Bu sebeple nesneleri sürekli üretmemek için ilk üretilen nesneyi "Databases" adlı bir havuza
     ekliyoruz. Daha sonra tekrar bir veritabanı işlemi yapılmak istendiğinde tekrar nesne üretmek yerine nesne
     "Databases" havuzundan çekiliyor. Ancak havuzda nesne yok ise yeni üretilerek kullanılıyor ve kullanıldıktan sonra
     havuza ekleniyor. Havuzun kapasitesi 3 olarak belirlendi.
     */
    class Program
    {
        static void Main(string[] args)
        {
            ObjectPool<VeriTabanlari> veritabaniObjesi = new ObjectPool<VeriTabanlari>(); //"ObjectPool sınıfından bir nesne üretiliyor
            VeriTabanlari sqlVeritabani = veritabaniObjesi.Get(); //Sql veritabanı işlemi için bir nesne isteniyor(havuzda var ise havuzdan çekilir yok ise üretilir)
            veritabaniObjesi.Release(sqlVeritabani);//Üretilen ya da çekilen nesne tekrar veri havuzuna ekleniyor
            sqlVeritabani.SqlDatabase();//Sql veritabanına ulaşılıyor
            VeriTabanlari oracleVeritabani = veritabaniObjesi.Get();//Oracle veritabanı işlemi için bir nesne isteniyor(havuzda var ise havuzdan çekilir yok ise üretilir)
            oracleVeritabani.Oracle();//Oraclle veritabanına ulaşılıyor
            VeriTabanlari mySqlVeritabani = veritabaniObjesi.Get();//MySql veritabanı işlemi için bir nesne isteniyor(havuzda var ise havuzdan çekilir yok ise üretilir)
            mySqlVeritabani.MySql();//Mysql veritabanına ulaşılıyor

            Console.ReadKey();
        }
    }

    public class ObjectPool<T> where T : new()
    {
        private readonly ConcurrentBag<T> DatabaseObjects = new ConcurrentBag<T>(); // Veri havuzu üretildi
        private int counter = 0;//Kaç nesne üretildiğini bilmek için sayaç oluşturuldu
        private int max = 3;//Databases havuzunun kapasitesi için max değer oluşturuldu
        public void Release(T Database)//Kullanılan nesneyi havuza ekler
        {
            if (counter < max)//Havuzda yer var ise
            {
                DatabaseObjects.Add(Database);
                Console.WriteLine("Nesne Havuzuna Eklendi");
                counter++;
            }
        }
        public T Get()//Nesne istemek için
        {
            T Database;
            if (DatabaseObjects.TryTake(out Database))//Havuzda nesne var ise havuzdan çekilir
            {
                counter--;
                Console.WriteLine("Nesne Havuzdan Çekildi");
                return Database;
            }
            else//Havuzda nesne yok ise yeni üretilir
            {
                T obj = new T();
                DatabaseObjects.Add(obj);
                counter++;
                Console.WriteLine("Yeni Nesne Üretildi");
                return obj;
            }
        }

    }


    class VeriTabanlari
    {
        static object kontrol = new object();
        public void SqlDatabase()
        {

            //Gerekli Sql Veritabanı Kodları
            Console.WriteLine("SqlDatabase Veritabanı Seçildi");


        }

        public void MySql()
        {

            //Gerekli MySql Veritabanı Kodları
            Console.WriteLine("MySql Veritabanı Seçildi");


        }

        public void Oracle()
        {

            //Gerekli Oracle Veritabanı Kodları
            Console.WriteLine("Oracle Veritabanı Seçildi");

        }


    }
}
