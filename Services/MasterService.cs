using System;
using System.Collections.Generic;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Services
{
    public class MasterService
    {
        private readonly BookRepository repo;

        public MasterService(BookRepository rep) {
            repo = rep;
        }

        public List<Category> FindAllCategories() {
		    return repo.Transaction(() => Category.FindAll(repo));
	    }
	
	    public void RegisterCategory(Category category) {
		    if(!IsExistCategory(category)) {
			    category.Save(repo);
		    }
	    }
        
        public Category GetCategory(int id) {
		    return Category.FindById(repo, id);
	    }

	    private Boolean IsExistCategory(Category category) {
		    Category c = Category.FindByName(repo, category.Name);
		    return (c == null) ? false : true;
	     }
	
	     public List<Format> FindAllFormats() {
		    return repo.Transaction(() => Format.FindAll(repo));
	    }
	
	    public void RegisterCategory(Format format) {
		    if(!IsExistFormat(format)) {
			    format.Save(repo);
		    }
	    }
        
        public Format GetFormat(int id) {
		    return Format.FindById(repo, id);
	    }

	    private Boolean IsExistFormat(Format format) {
		    Format f = Format.FindByName(repo, format.Name);
		    return (f == null) ? false : true;
	     }
    }
}