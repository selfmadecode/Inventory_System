﻿using System;
using System.Collections.Generic;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppWebUi.Models
{
    public class ReportViewModel
    {
        public decimal TotalRevenueForReport { get; set; }

        public List<Drug> ReportDrugs { get; set; }
        
        public List<Order> Orders { get; set; }
    }
}