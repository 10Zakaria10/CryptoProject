using cryptoProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace cryptoProject.Controllers
{
    public class AcceuilController : Controller
    {
        // Database context
        QuestionnaireDBEntities qstDB = new QuestionnaireDBEntities();
        
        
        // GET: Acceuil
        public ActionResult Index()
        {
            // juste dans la phase des tests : a chaque fois je refai le score a 0 
            var x = from c in qstDB.Questions select c;
            var cat = from c in qstDB.Categories select c;

            foreach (Question qst in x)
            {
                {
                    var categorie = (from c in qstDB.Categories where c.id == qst.id_cat select c).SingleOrDefault();

                    categorie.score = 0;

                }

            }
            qstDB.SaveChanges();


            ViewBag.questions = x;
            ViewBag.categories = cat;

            return View(x);
        }

        [HttpPost]
        public ActionResult scorePage(FormCollection formx)
        {
            // les list des questions
            var qsts = from c in qstDB.Questions select c;
            List<Question> listQst = qsts.ToList();

            // update score f bd
            foreach (Question qst in qsts)
            {
                if (formx[qst.id.ToString()] != null)
                {
                    var categorie = (from c in qstDB.Categories where c.id == qst.id_cat select c).SingleOrDefault();

                    categorie.score++;

                } // j'affiche la solution que des questions non  selectione 
                else
                {
                    listQst.Remove(qst);
                }

            }
            qstDB.SaveChanges();

            // get categories
            var cat = from c in qstDB.Categories select c;
            foreach (Categorie categorie in cat)
            {
                String key = categorie.nom;
                //total des questions de chaque categorie
                ViewData[key] = (double)(from c in qstDB.Questions where c.id_cat == categorie.id select c).Count();
            }
            ViewBag.categories = cat;


            return View(listQst);
        }

        // pour Pie chart
        public ActionResult BillChart()
        {
            String[] catName = (from c in qstDB.Categories select c.nom).ToArray();
            var catScore = (from c in qstDB.Categories select c.score).ToArray();


            String myTheme =
                @"<Chart BackColor=""Transparent"">
                                        <ChartAreas>
                                            <ChartArea Name =""Default"" BackColor=""Transparent""></ChartArea>
                                        </ChartAreas>
                   </Chart>";


            new Chart(width: 350, height: 350, theme: myTheme)
                .AddSeries(
                    chartType: "pie",
                    xValue: catName,
                    yValues: catScore
                )
                .Write("png"); 

            return null;
        }

    }
}