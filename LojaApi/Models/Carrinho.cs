﻿namespace LojaApi.Models
{
    public class Carrinho
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set;}
    }
}
