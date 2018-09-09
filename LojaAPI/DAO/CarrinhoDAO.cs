using LojaAPI.Models;
using System.Collections.Generic;

namespace LojaAPI.DAO
{
    public class CarrinhoDAO
    {
        private static Dictionary<long, Carrinho> banco = new Dictionary<long, Carrinho>();
        private static long contador = 0;

        static CarrinhoDAO()
        {
            Produto videogame = new Produto(6237, 4000, "Videogame 4", 1);
            Produto esporte = new Produto(3467, 60, "Jogo de esporte", 2);
            Carrinho carrinho = new Carrinho();
            carrinho.Adiciona(videogame);
            carrinho.Adiciona(esporte);
            carrinho.Endereco = "Rua Vergueiro 3185, 8 andar, São Paulo";
            carrinho.Id = ++contador;
            banco.Add(1, carrinho);
        }

        public void Adiciona(Carrinho carrinho)
        {
            carrinho.Id = ++contador;
            banco.Add(carrinho.Id, carrinho);
        }

        public Carrinho Buscar(long id)
        {
            return banco[id];
        }

        public void Remove(long id)
        {
            banco.Remove(id);
        }
    }
}