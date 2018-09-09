using System;
using LojaAPI.DAO;
using LojaAPI.Models;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;

namespace LojaAPI.Controllers
{
    public class CarrinhoController : ApiController
    {
        public HttpResponseMessage Get(long id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new CarrinhoDAO().Buscar(id));
            }
            catch (KeyNotFoundException)
            {
                HttpError err = new HttpError(string.Format("O carrinho {0} não foi localizado", id));
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, err);
                return response;
            }
            catch (Exception)
            {
                HttpError err = new HttpError("Erro não previsto");
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                return response;
            }
        }

        public HttpResponseMessage Post([FromBody]Carrinho carrinho)
        {
            try
            {
                CarrinhoDAO dao = new CarrinhoDAO();
                dao.Adiciona(carrinho);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "carrinho", id = carrinho.Id }));
                return response;
            }
            catch (Exception)
            {
                HttpError err = new HttpError("Erro não previsto");
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                return response;
            }
        }

        [Route("api/carrinho/{idCarrinho}/produto/{idProduto}")]
        public HttpResponseMessage Delete([FromUri]long idCarrinho, [FromUri]long idProduto)
        {
            try
            {
                new CarrinhoDAO().Buscar(idCarrinho).Remove(idProduto);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (KeyNotFoundException)
            {
                HttpError err = new HttpError(string.Format("O carrinho {0} não foi localizado", idCarrinho));
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, err);
                return response;
            }
            catch (Exception)
            {
                HttpError err = new HttpError("Erro não previsto");
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                return response;
            }
        }

        [Route("api/carrinho/{idCarrinho}/produto/{idProduto}/quantidade")]
        public HttpResponseMessage Put([FromBody]Produto produto, [FromUri]long idCarrinho, [FromUri]long idProduto)
        {
            try
            {
                new CarrinhoDAO().Buscar(idCarrinho).TrocaQuantidade(produto);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (KeyNotFoundException)
            {
                HttpError err = new HttpError(string.Format("O carrinho {0} não foi localizado", idCarrinho));
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, err);
                return response;
            }
            catch (Exception)
            {
                HttpError err = new HttpError("Erro não previsto");
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                return response;
            }
        }
    }
}
