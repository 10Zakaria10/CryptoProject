//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cryptoProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        public int id { get; set; }
        public string nom { get; set; }
        public Nullable<int> id_cat { get; set; }
        public Nullable<int> coeficiant { get; set; }
        public Nullable<int> id_sol { get; set; }
    
        public virtual Categorie Categorie { get; set; }
        public virtual Solution Solution { get; set; }
    }
}