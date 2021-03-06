﻿using cryptoProject.Models;
using cryptoProject.Raport;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        public void initializ()
        {
            var x = from c in qstDB.Questions select c;

            foreach (Question qst in x)
            {
                {
                    var categorie = (from c in qstDB.Categories where c.id == qst.id_cat select c).SingleOrDefault();

                    categorie.score = 0;
                    qst.reponse = 0;
                }

            }
            qstDB.SaveChanges();

        }

        public void addQst()
        {
            /*
            //add solutiion 
            Solution solution = new Solution();
            solution.description = "SolA";

            qstDB.Solutions.Add(solution);
            //add categorie
            Categorie cat = new Categorie();
            cat.nom = "Securite logiciel";
            cat.score = 0;
            qstDB.Categories.Add(cat);
            */        
    
            // add qst

            Question qst = new Question();
            qst.reponse = 0;
            qst.qst = "D";
            qst.coeficiant = 2;
            qst.id_cat = 1;
            qst.id_sol = 4;
            qstDB.Questions.Add(qst);

        }
        // GET: Acceuil
        public ActionResult Index()
        {
           // addQst();
            // juste dans la phase des tests : a chaque fois je refai le score a 0 
            initializ();

            var x = from c in qstDB.Questions select c;
            var cat = from c in qstDB.Categories select c;

           

            ViewBag.questions = x;
            ViewBag.categories = cat;

            return View(x);
        }

        [HttpPost]
        public ActionResult scorePage(FormCollection formx)
        {
            initializ();

            // les list des questions
            var qsts = from c in qstDB.Questions select c;
            List<Question> listQst = qsts.ToList();

            // update score f bd
            foreach (Question qst in qsts)
            {
                if (formx[qst.id.ToString()] != null)
                {
                    var categorie = (from c in qstDB.Categories where c.id == qst.id_cat select c).SingleOrDefault();

                    categorie.score += qst.coeficiant;
                    qst.reponse = 1;
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
                String totalKey = categorie.nom;
                //somme des score des questions de chaque categorie
                double total = (double)(from c in qstDB.Questions where c.id_cat == categorie.id select c).Sum(coef => coef.coeficiant);
                ViewData[totalKey] = total;
                //somme des questions de categorie
                String sumExistlKey = categorie.nom + "sumExist";
                int sumExist = (from c in qstDB.Questions where c.id_cat == categorie.id && c.reponse == 1 select c).Count();
                ViewData[sumExistlKey] = sumExist;
                //somme des questions non valide de categorie
                String nonExistlKey = categorie.nom + "nonExist";
                int nonExist = (from c in qstDB.Questions where c.id_cat == categorie.id && c.reponse != 1 select c).Count();
                ViewData[nonExistlKey] = nonExist;
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
        /*
        public ActionResult ExportRap()
        {
            
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Raport/zak.rpt")));
                   
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "AuditRapport.pdf");
            }
            catch
            {
                throw; 
            }
        }
        
    */
        public ActionResult Report()
        {
            List<Question> x =( from c in qstDB.Questions where c.reponse == 0 select c ).ToList();
            QuestionReport qstReport = new QuestionReport();
            byte[] abytes = qstReport.PrepareReport(x);
            return File(abytes, "application/pdf");
        }
        

    }
}