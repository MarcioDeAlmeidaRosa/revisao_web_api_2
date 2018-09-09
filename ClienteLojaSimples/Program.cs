using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ClienteLojaSimples
{
    class Program
    {
        private static string URL = "http://localhost:53712/api";

        static void Main(string[] args)
        {
            string controleCarrinho = "/carrinho";
            Console.WriteLine("******Iniciando testes************");
            #region
            //Saindo o conteúdo no formato XML
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Retornando via XML:");
            Console.WriteLine(GetCarrinho(string.Format("{0}{1}/1", URL, controleCarrinho), "application/xml"));
            //Saindo o conteúdo no formato JSON
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Retornando via JSON:");
            Console.WriteLine(GetCarrinho(string.Format("{0}{1}/1", URL, controleCarrinho), "application/json"));
            //Saindo o conteúdo pelo formato default
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Retornando pelo formado default:");
            Console.WriteLine(GetCarrinho(string.Format("{0}{1}/1", URL, controleCarrinho)));
            //Incluindo novo carrinho - via JSON
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Incluindo novo carrinho - via JSON");
            string novoCarrinhoJSON = @"{
            'produtos':[{
                    'id':6237,
                    'preco':5000.0,
                    'nome':'PlayStation 4',
                    'quantidade':1
                },{
                    'id':3467,
                    'preco':3500.0,
                    'nome':'XBox',
                    'quantidade':1
                }],
                'endereco':'Rua Vergueiro 4000, 10 andar, São Paulo',
            }";
            Console.WriteLine(NovoCarrinho(string.Format("{0}{1}", URL, controleCarrinho), novoCarrinhoJSON, "application/json"));
            //Incluindo novo carrinho - via XML
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Incluindo novo carrinho - via XML");
            string novoCarrinhoXML = @"
            <Carrinho xmlns:i='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://schemas.datacontract.org/2004/07/LojaAPI.Models'>
                <Endereco>Avenida Paulista 563, 15 andar, São Paulo</Endereco>
                <Produtos>
                    <Produto>
                        <Id>56325</Id>
                        <Nome>Citroen C4</Nome>
                        <Preco>40000.00</Preco>
                        <Quantidade>1</Quantidade>
                    </Produto>
                    <Produto>
                        <Id>1562</Id>
                        <Nome>Rodas de Liga Leve</Nome>
                        <Preco>5000</Preco>
                        <Quantidade>4</Quantidade>
                    </Produto>
                </Produtos>
            </Carrinho>";
            Console.WriteLine(NovoCarrinho(string.Format("{0}{1}", URL, controleCarrinho), novoCarrinhoXML, "application/xml"));
            //Saindo o conteúdo no formato XML
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Retornando via XML:");
            Console.WriteLine(GetCarrinho(string.Format("{0}{1}/2", URL, controleCarrinho), "application/xml"));
            //Saindo o conteúdo no formato JSON
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Retornando via JSON:");
            Console.WriteLine(GetCarrinho(string.Format("{0}{1}/3", URL, controleCarrinho), "application/json"));
            //Executando consulta para registro que não existe
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Retornando via JSON:");
            Console.WriteLine(GetCarrinho(string.Format("{0}{1}/3000", URL, controleCarrinho), "application/json"));
            //Executando remover do produto no carrinho
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Removendo produto com retorno JSON:");
            Console.WriteLine(RemoveProduto(string.Format("{0}{1}/3/produto/6237", URL, controleCarrinho), "application/json"));
            //Executando remover do produto no carrinho
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Atualiza produto com retorno JSON:");
            Console.WriteLine(AtualizaProduto(string.Format("{0}{1}/3/produto/1562/quantidade", URL, controleCarrinho), @"{
            'id': 1562,
            'preco': 50000,
            'nome': 'Rodas de Liga Leve',
            'quantidade': 11
        }", "application/json"));
            #endregion
            Console.Read();
        }

        private static string GetCarrinho(string url, string accept = null)
        {
            try
            {
                string conteudoRetorno = string.Empty;
                //Criando um Web Request para um endereço
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //Configurando o método de chamada via GET
                request.Method = "GET";
                if (!string.IsNullOrWhiteSpace(accept)) request.Accept = accept;
                //Criando um Web Response para o conteúdo retornado da requisição anterior
                WebResponse response = request.GetResponse();
                //Recuperando o stream do retorno
                using (Stream responseStream = response.GetResponseStream())
                {
                    //Lendo o Stream do retorno do serviço
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    //Atrinbuindo o conteúdo enviado pelo serviço a variável que irá armazenar a string enviada
                    conteudoRetorno = streamReader.ReadToEnd();
                }
                //Retornando a string na tela
                return conteudoRetorno;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject(resp);
                return obj.message;
            }
        }

        private static string NovoCarrinho(string url, string body, string accept)
        {
            try
            {
                string conteudoRetorno = string.Empty;

                //Criando um Web Request para um endereço
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //Configurando o método de chamada via GET
                request.Method = "POST";
                if (!string.IsNullOrWhiteSpace(accept)) request.Accept = accept;

                //transformando mensagem string em array de bite
                byte[] mensagem = Encoding.UTF8.GetBytes(body);

                //Escrevendo no request o body à ser enviado
                request.GetRequestStream().Write(mensagem, 0, mensagem.Length);

                //Atribuindo qual o content type da mensagem sendo enviada
                if (!string.IsNullOrWhiteSpace(accept)) request.ContentType = accept;

                //Recupera o status code e o location do registro criado
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                conteudoRetorno = string.Format(@"
StatusCode = {0}
StatusDescription = {1}
Location = {2}",
                    response.StatusCode,
                    response.StatusDescription,
                    response.Headers["Location"]);

                //Retornando a string na tela
                return conteudoRetorno;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject(resp);
                return obj.message;
            }
        }

        private static string RemoveProduto(string url, string accept)
        {
            try
            {
                string conteudoRetorno = string.Empty;

                //Criando um Web Request para um endereço
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //Configurando o método de chamada via GET
                request.Method = "DELETE";
                if (!string.IsNullOrWhiteSpace(accept)) request.Accept = accept;

                //Recupera o status code e o location do registro criado
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                conteudoRetorno = string.Format(@"
StatusCode = {0}
StatusDescription = {1}
Location = {2}",
                    response.StatusCode,
                    response.StatusDescription,
                    response.Headers["Location"]);

                //Retornando a string na tela
                return conteudoRetorno;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject(resp);
                return obj.message;
            }
        }

        private static string AtualizaProduto(string url, string produto, string accept)
        {
            try
            {
                string conteudoRetorno = string.Empty;

                //Criando um Web Request para um endereço
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //Configurando o método de chamada via GET
                request.Method = "PUT";
                if (!string.IsNullOrWhiteSpace(accept)) request.Accept = accept;

                //transformando mensagem string em array de bite
                byte[] mensagem = Encoding.UTF8.GetBytes(produto);

                //Escrevendo no request o body à ser enviado
                request.GetRequestStream().Write(mensagem, 0, mensagem.Length);

                //Atribuindo qual o content type da mensagem sendo enviada
                if (!string.IsNullOrWhiteSpace(accept)) request.ContentType = accept;

                //Recupera o status code e o location do registro criado
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                conteudoRetorno = string.Format(@"
StatusCode = {0}
StatusDescription = {1}
Location = {2}",
                    response.StatusCode,
                    response.StatusDescription,
                    response.Headers["Location"]);

                //Retornando a string na tela
                return conteudoRetorno;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject(resp);
                return obj.message;
            }
        }
    }
}
