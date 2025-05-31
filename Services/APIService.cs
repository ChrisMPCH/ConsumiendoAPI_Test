using ControlEscolarCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;
    // Obtener la URL desde el archivo  configuración
    private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? throw new InvalidOperationException("La clave 'ApiBaseUrl' no está configurada en AppSettings.");

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<List<Estudiante>> GetEstudiantesAsync(bool soloActivos = true,
                                                          int tipoFecha = 2,
                                                          DateTime? fechaInicio = null,
                                                          DateTime? fechaFin = null)
    {
        try
        {
            string endpoint = "estudiantescontrollerapi_test/list_estudiantes";
            string queryString = $"?soloActivos={soloActivos}&tipoFecha={tipoFecha}";

            if (fechaInicio.HasValue)
                queryString += $"&fechaInicio={fechaInicio.Value:yyyy-MM-dd}";

            if (fechaFin.HasValue)
                queryString += $"&fechaFin={fechaFin.Value:yyyy-MM-dd}";

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + endpoint + queryString);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Estudiante>>(json);
            }
            else
            {
                // Manejar errores
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener estudiantes: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            // Loguear y relanzar la excepción
            Console.WriteLine($"Error en API: {ex.Message}");
            throw;
        }
    }
}