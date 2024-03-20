using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(WebBanDoDienTu.Startup))]
namespace WebBanDoDienTu
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.MapSignalR();
        }
    }
}