﻿using Amazon.Runtime.Internal;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQueries : IRequest<Pagination<ProductResponse>>
    {
        public CatalogSpecParams CatalogSpecParams { get; set; }
        public GetAllProductsQueries(CatalogSpecParams catalogSpecParams)
        {
            CatalogSpecParams = catalogSpecParams; 
        }
    }
}
