﻿using App.Models;
using Scaffold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Controllers
{
    public class BaseTransferRecapitulationController<TRecapitulation> : ReadOnlyController<TRecapitulation, long>
        where TRecapitulation: BaseTransferRecapitulation, new()
    {
        public BaseTransferRecapitulationController(DB dbContext)
            : base(dbContext)
        {
        }

        protected override IQueryable<TRecapitulation> ApplyQuery(IQueryable<TRecapitulation> query)
        {
            var parentID = GetQueryString<long?>("ParentID");
            return query.Where(t => t.ParentRegionID == parentID.Value || t.RegionID == parentID.Value);
        }
    }
    public class TransferRecapitulationController : BaseTransferRecapitulationController<TransferRecapitulation>
    {
        public TransferRecapitulationController(DB dbContext)
            : base(dbContext)
        {
        }
    }
    public class FrozenTransferRecapitulationController : BaseTransferRecapitulationController<FrozenTransferRecapitulation>
    {
        public FrozenTransferRecapitulationController(DB dbContext)
            : base(dbContext)
        {
        }
    }
}