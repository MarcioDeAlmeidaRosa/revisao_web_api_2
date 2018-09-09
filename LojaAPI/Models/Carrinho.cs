using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace LojaAPI.Models
{
    public class Carrinho
    {
        public List<Produto> Produtos { get; set; }
        public string Endereco { get; set; }
        public long Id { get; set; }

        public Carrinho()
        {
            Produtos = new List<Produto>();
        }

        public string ToXML()
        {
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(Carrinho));
            StringWriter stringWriter = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                xmlSerialize.Serialize(writer, this);
                return stringWriter.ToString();
            }
        }

        public void Adiciona(Produto produto)
        {
            Produtos.Add(produto);
        }

        public void Remove(long id)
        {
            Produto produto = Produtos.FirstOrDefault(p => p.Id == id);
            Produtos.Remove(produto);
        }

        public void Troca(Produto produto)
        {
            Remove(produto.Id);
            Adiciona(produto);
        }

        public void TrocaQuantidade(Produto produto)
        {
            Produto produtoFind = Produtos.FirstOrDefault(p => p.Id == produto.Id);
            produtoFind.Preco = (produtoFind.Preco / produtoFind.Quantidade) * produto.Quantidade;
            produtoFind.Quantidade = produto.Quantidade;
        }

        public void TrocaEndereco(string endereco)
        {
            Endereco = endereco;
        }
    }
}