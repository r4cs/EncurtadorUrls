using cp2.Entities;
using cp2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cp2.Services
{

    public class ApiService
    {
        private readonly EncurtadorDbContext _context;
        private readonly EncurtadorUrlService _encurtadorUrlService;

        
        public ApiService(EncurtadorDbContext context, 
                          EncurtadorUrlService encurtadorUrlService)
        {
            _context = context;
            _encurtadorUrlService = encurtadorUrlService;
        }

        public async Task<EncurtadorUrl?> Create(EncurtadorUrlRequest _request, HttpContext httpContext)
        {
            if (!Uri.TryCreate(_request.Url, UriKind.Absolute, out _))
            {
                return null;
            }

            var codigo = await _encurtadorUrlService.GerarCodigoUnico();
            var apiVersion = httpContext.GetRequestedApiVersion().ToString();
            var encurtadorUrl = new EncurtadorUrl
            {
                Id = Guid.NewGuid(),
                UrlLonga = _request.Url,
                Codigo = codigo,
                UrlCurta = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/v{apiVersion}/r/{codigo}",
                CriadoEm = DateTime.Now
            };
            _context.EncurtadorUrls.Add(encurtadorUrl);
            await _context.SaveChangesAsync();
            return encurtadorUrl;
        }


        public async Task<EncurtadorUrl?> Read(string codigo)
        {
            return await _context.EncurtadorUrls.FirstOrDefaultAsync(s => s.Codigo == codigo);
        }

        // public async Task<List<EncurtadorUrl>> GetAll()
        // {
        //     return await _context.EncurtadorUrls.ToListAsync();
        // }
        
        public async Task<List<EncurtadorUrl>> GetAll(string orderBy = null)
        {
            var query = _context.EncurtadorUrls.AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "url longa":
                        query = query.OrderBy(e => e.UrlLonga);
                        break;
                    case "criado em":
                        query = query.OrderBy(e => e.CriadoEm);
                        break;
                    case "url curta":
                        query = query.OrderBy(e => e.UrlCurta);
                        break;
                    case "codigo":
                        query = query.OrderBy(e => e.Codigo);
                        break;
                    default:
                        query = query.OrderBy(e => e.Id);
                        break;
                }
            }

            return await query.ToListAsync();
        }
        
        

        public async Task<bool> Update(string codigo, string novaUrl)
        {
            var existingUrl = await _context.EncurtadorUrls.FirstOrDefaultAsync(s => s.Codigo == codigo);

            if (existingUrl == null)
            {
                return false;
            }

            existingUrl.UrlLonga = novaUrl;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> Delete(string codigo)
        {
            var encurtadorUrl = await _context.EncurtadorUrls.FirstOrDefaultAsync(s => s.Codigo == codigo);
            if (encurtadorUrl == null)
            {
                return false;
            }

            _context.EncurtadorUrls.Remove(encurtadorUrl);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}