using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.Test.Tools.Fakes
{
    public static class ControllerFake
    {
        public static T GetController<T>(params object[] args)
            where T : Controller
        {
            var controller = (T)Activator.CreateInstance(typeof(T), args);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Form = new FormCollection(null);
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            return controller;
        }
    }
}
