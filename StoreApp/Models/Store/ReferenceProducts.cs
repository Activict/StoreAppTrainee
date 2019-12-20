using System.Web.Mvc;

namespace StoreApp.Models.Store
{
    public class ReferenceProducts
    {
        public SelectList Units { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Producers { get; set; }
    }
}