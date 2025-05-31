using System;
using System.Configuration;

namespace ConsumiendoAPI_Test
{
    public partial class Form1 : Form
    {
        private readonly ApiService _apiService;
        public Form1()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }


        private async void Consulta()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var estudiantes = await _apiService.GetEstudiantesAsync(true, 2, DateTime.Parse("01/01/2025"), DateTime.Parse("10/05/2025"));

                // Proyectar solo los campos que quieres mostrar
                var datosPlano = estudiantes.Select(e => new
                {
                    e.Id,
                    e.Matricula,
                    e.Semestre,
                    FechaAlta = e.FechaAlta.ToString("yyyy-MM-dd"),
                    FechaBaja = e.FechaBaja?.ToString("yyyy-MM-dd") ?? "",
                    e.Estatus,
                    Nombre = e.DatosPersonales.NombreCompleto,

                    Correo = e.DatosPersonales.Correo,
                    Telefono = e.DatosPersonales.Telefono,
                    Curp = e.DatosPersonales.Curp,
                    FechaNacimiento = e.DatosPersonales.FechaNacimiento?.ToString("yyyy-MM-dd") ?? "",
                    EstatusPersona = e.DatosPersonales.Estatus ? "Activo" : "Inactivo"
                }).ToList();

                dataGridView1.DataSource = datosPlano;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar estudiantes: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            Consulta();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Consulta();
        }
    }
}
