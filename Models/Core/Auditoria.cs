namespace Banco.Models.Core
{
    public class Auditoria
    {
        public int Id { get; set; }
        public string TabelaAfetada { get; set; }
        public int RegistroId { get; set; }
        public string Acao { get; set; }
        public string Detalhes { get; set; }
        public DateTime DataHora { get; set; }
        public string UsuarioId { get; set; }
    }
}