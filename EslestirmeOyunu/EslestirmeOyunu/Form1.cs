using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EslestirmeOyunu
{
    public partial class Form1 : Form
    {

        Random rnd = new Random();
        Label ilk = null;
        Label ikinci = null;
        Timer baslangicTimer = new Timer(); // başlangıçta resimleri göstermek için timer oluşturduk.
        Timer secimTimer = new Timer();  //eşleşme kontrolü için timer
        Timer kartSecimSuresi = new Timer();//kart seçim için timer oluşturduk.


        int oyuncu1Sayı = 0;
        int oyuncu2Sayı = 0;

        bool birinciOyuncuSırada = true; //sırayı takip etmek için


        List<string> icons = new List<string>()
        {

            "a","a","N","N","M","M","k","k","w","w","!","!","b","b","z","z","p","p","y","y",
            ",",",","d","d","t","t","r","r","e","e","h","h","j","j","q","q","o","o","i","i"
        };

        
        private void resimAta()
        {
            foreach(Control etiket in tableLayoutPanel1.Controls)
            {
                Label resimEtk=etiket as Label;

                if (resimEtk != null)
                {
                    int rastgele = rnd.Next(icons.Count);
                    resimEtk.Text = icons[rastgele]; //text'e rastgele sayılar atadık
                    resimEtk.ForeColor = Color.Black; // Başlangıçta şekilleri açık göster         
                    icons.RemoveAt(rastgele); //seçtiğini kaldırıyor listeden;
                }


            }
            // Başlangıçta şekillerin 5 saniye açık kalmasını sağlayacak
            baslangicTimer.Interval = 5000; // 5 saniye (5000 milisaniye)
            baslangicTimer.Tick += BaslangicTimer_Tick;//5 saniye dolduğunda methot çalıştırıyor
            baslangicTimer.Start(); // Timer'ı başlatıyoruz


        }
        private void BaslangicTimer_Tick(object sender, EventArgs e)
        {
            baslangicTimer.Stop(); // Timer'ı durdur

            // Tüm etiketlerin görünürlüğünü kapat
            foreach (Control etiket in tableLayoutPanel1.Controls)
            {
                Label resimEtk = etiket as Label;

                if (resimEtk != null)
                {
                    resimEtk.ForeColor = resimEtk.BackColor; // Şekilleri kapat
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            resimAta();

            kartSecimSuresi.Tick += KartSecimSuresiTimer_Tick;
            secimTimer.Tick += SecimTimer_Tick;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void labek_Click(object sender, EventArgs e)
        {
            
            if (secimTimer.Enabled == true)
            {
                return; // zamanlayıcı aktifse işlem yapılmaz.
            }



            Label secilenEtiket = sender as Label; //hangi label'a tıkladığın sender da tutuluyor onuda secilenEtikete atadık.

            if (secilenEtiket != null) 
            {

                if (secilenEtiket.ForeColor == Color.Black) //secilen kısım zaten açılmışsa birşey yapma.
                {
                    return;
                }

                //eğer ilk kart seçilmemişse ilk kartı seç
                if (ilk == null)
                {
                    ilk = secilenEtiket;
                    ilk.ForeColor = Color.Black;

                    kartSecimSuresi.Interval = 5000;//ilk kart açıldığında 5 saniye bekleme süresi 
                    //kartSecimSuresi.Tick += KartSecimSuresiTimer_Tick;
                    kartSecimSuresi.Start();//ilk kart seçildiği için 5 saniye süreyi başlattık.

                    return;
                }
                ikinci = secilenEtiket;
                ikinci.ForeColor = Color.Black;

                // 5 saniye içinde ikinci kart açıldığı için zamanlayıcıyı durdur
                kartSecimSuresi.Stop();


                if (ilk.Text == ikinci.Text) //aynı ise açık kalıcak
                {
                    if (birinciOyuncuSırada)
                    {
                        oyuncu1Sayı++;
                        button1.Text = oyuncu1Sayı.ToString();
                    }
                    else
                    {
                        oyuncu2Sayı++;
                        button2.Text = oyuncu2Sayı.ToString();
                    }

                    ilk = null;
                    ikinci = null;
                    oyunBittiMi();
                    //return;
                }
                else
                {

                    // Eşleşme yoksa, 5 saniye bekleyip resimleri kapat
                    secimTimer.Interval = 5000;
                    //secimTimer.Tick += SecimTimer_Tick;
                    secimTimer.Start();

                }

            }

        }
           //kart seçimi yapılmazsa sıra değiştir
        private void KartSecimSuresiTimer_Tick(object sender, EventArgs e)
        {
            kartSecimSuresi.Stop();

            // İlk kart açıldı ama ikinci kart seçilmedi, sırayı değiştir
            birinciOyuncuSırada = !birinciOyuncuSırada;

            if (birinciOyuncuSırada)
            {
                MessageBox.Show("5 saniye süresi doldu Birinci oyuncunun sırası");
            }
            else
            {
                MessageBox.Show("5 saniye süresi doldu İkinci oyuncunun sırası");
            }

            // İlk kartı kapat ve sıfırla
            if (ilk != null)
            {
                
                ilk.ForeColor = ilk.BackColor;
                ilk = null;
               
            }
        }
        private void SecimTimer_Tick(object sender, EventArgs e)
        {
            secimTimer.Stop();

            // ilk ve ikinci'nin null olmadığından emin olun
            if (ilk != null && ikinci != null)
            {
                // Eşleşme başarısız, resimleri kapat
                ilk.ForeColor = ilk.BackColor;
                ikinci.ForeColor = ikinci.BackColor;


                //sıradaki oyuncuyu değiştir
                birinciOyuncuSırada = !birinciOyuncuSırada;

                if (birinciOyuncuSırada)
                {
                    MessageBox.Show("Birinci oyuncu sırada");
                }
                else
                {
                    MessageBox.Show("İkinci oyuncu sırada");
                }
            }
            

           
          

            // Değişkenleri temizleyin
            ilk = null;
            ikinci = null;
        }
        private void oyunBittiMi()
        {

            if (oyuncu1Sayı >= 11)
            {
                MessageBox.Show("Oyuncu1 kazandı");
                Close();
            }else if (oyuncu2Sayı >= 11)
            {
                MessageBox.Show("Oyuncu2 kazandı");
                Close();
            }

            foreach(Control etiket in tableLayoutPanel1.Controls)
            {
                Label resimEtk = etiket as Label;

                if (resimEtk != null)
                {
                    if (resimEtk.ForeColor == resimEtk.BackColor)
                    {
                        return;  //kapalı kart varsa bitmemiştir
                    }

                }

            }
            if (oyuncu1Sayı == oyuncu2Sayı)
            {
                MessageBox.Show("Oyun 10-10 berabere bitti");
            }
            Close();
            

        }
        
      

    }
}
