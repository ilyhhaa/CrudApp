using CrudApp.Models;

namespace CrudApp.ViewModels
{
    public class IndexViewModel
    {
       public IEnumerable<Thing> Things { get; set; }

        public IEnumerable<MongoThings> MongoThings { get; set; }

        public string CurrentSearch {  get; set; }
    }
}
