using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class CartIndexVM
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}