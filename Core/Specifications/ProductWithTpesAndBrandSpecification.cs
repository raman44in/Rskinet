using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithTpesAndBrandSpecification : BaseSpecification<Product>
    {
        Product product=new Product ();

        public ProductWithTpesAndBrandSpecification(int id) : base(x=>x.Id==id)
        {
            AddIncludes(x=>x.ProductType);
            AddIncludes(x=>x.ProductBrand);  
        }
        public ProductWithTpesAndBrandSpecification()
        {
            AddIncludes(x=>x.ProductType);
            AddIncludes(x=>x.ProductBrand);
        }
    }
}