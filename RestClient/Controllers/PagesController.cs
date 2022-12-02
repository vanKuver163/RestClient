using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using RestClient.Models;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace RestClient.Controllers
{
    public class PagesController : Controller
    {
        //Get /pages
        public async Task <IActionResult> Index()
        {
            List<Page> pages = new List<Page>();
            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.GetAsync("https://localhost:5001/api/pages");
                string responce = await request.Content.ReadAsStringAsync();
                pages = JsonConvert.DeserializeObject<List<Page>>(responce);
            }
            return View(pages);
        }

        //Get /pages/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Page page = new Page();

            
            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.GetAsync($"https://localhost:5001/api/pages/{id}");
                string responce = await request.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(responce);
            }
            return View(page);
        }

        //Post /pages/edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");

                using var request = await httpClient.PutAsync($"https://localhost:5001/api/pages/{page.Id}", content);
                string responce = await request.Content.ReadAsStringAsync();
                
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        //Get /pages/create
        public IActionResult Create() => View();

        //Post /pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();
            page.Sorting = 100;

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), Encoding.UTF8, "application/json");

                using var request = await httpClient.PostAsync($"https://localhost:5001/api/pages", content);
                string responce = await request.Content.ReadAsStringAsync();

            }
            return RedirectToAction("Index");
        }

        //Get /pages/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            
            using (var httpClient = new HttpClient())
            {
                using var request = await httpClient.DeleteAsync($"https://localhost:5001/api/pages/{id}");               
            }
            return RedirectToAction("Index");
        }

    }
}
