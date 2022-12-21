using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormularioPersonas.Models;

namespace FormularioPersonas
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sende, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpClient client = new HttpClient();                    
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                PersonaUsuario DTO = new PersonaUsuario()
                {
                    Nombres = textBox1.Text,
                    Apellidos = textBox2.Text,
                    NumeroIdentificacion = textBox3.Text,
                    Email = textBox4.Text,
                    TipoIdentificacion = textBox5.Text,
                    Usuario = textBox6.Text,
                    Pass = textBox7.Text,
                };
          
                var url = await CreatePersonAsync(DTO);                                

            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }

        static async Task<Uri> CreatePersonAsync(PersonaUsuario DTO)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sende, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "https://localhost:7270/api/Home", DTO);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                MessageBox.Show("El Numero de identificación " + DTO.NumeroIdentificacion + " ya está registrado.");
            }
            else if(response.StatusCode == HttpStatusCode.OK) 
            {
                MessageBox.Show("Usuario registrado con exito.");
            }

            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }
    }
}
