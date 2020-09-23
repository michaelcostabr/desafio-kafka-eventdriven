namespace SiteReservas
{
    public class Reservas
    {

        /// <summary>
        /// Gets or Sets Localizador
        /// </summary>
        public string Localizador { get; set; }

        /// <summary>
        /// Gets or Sets Placa
        /// </summary>
        public string Carro { get; set; }

        /// <summary>
        /// Gets or Sets IdCLiente
        /// </summary>
        public string Cliente { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets IdPagamento
        /// </summary>
        public int? IdPagamento { get; set; }
    }
}
