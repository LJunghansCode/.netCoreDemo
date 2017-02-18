using System.Linq;
using Microsoft.AspNetCore.Mvc;
using blackBelt.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Humanizer;

namespace blackBelt.Controllers
{
    public class MainController : Controller
    {
        private MainContext _context;
         public MainController(MainContext context)
        {
            _context = context;
        }
        public void EndAuction(int auctId)
        {
            Auction endingAuction = _context.Auctions.FirstOrDefault(auct => auct.Id == auctId);
            User bidAuthor = _context.Users.FirstOrDefault(user => user.FirstName == endingAuction.Author);
            User bidWinner = _context.Users.FirstOrDefault(user => user.FirstName == endingAuction.BidId);
            bidWinner.Wallet -= endingAuction.HighestBid;
            bidAuthor.Wallet += endingAuction.HighestBid;
            _context.Auctions.Remove(endingAuction);
            _context.SaveChanges();

        }
        [HttpPost]
        [Route("/EndAuction")]
        public IActionResult EndEarly(int auctId)
        {
            EndAuction(auctId);
            return Redirect("/Main");
        }
        [HttpGet]
        [Route("/Main")]
        public IActionResult Main()
        {
            
            ViewBag.userFirstName = HttpContext.Session.GetString("userFirstName");
            User userInstance = _context.Users.FirstOrDefault(user => user.Id == (int)HttpContext.Session.GetInt32("userID"));
            if(ViewBag.userFirstName == null)
            {
                return Redirect("/");
            }
            ViewBag.Wallet = userInstance.Wallet;
            ViewBag.Now = DateTime.Now;    
            ViewBag.AllAuctions = _context.Auctions.ToList().OrderBy(auc=>auc.EndDate);
            foreach(Auction auct in ViewBag.AllAuctions)
            {
                if(auct.EndDate < DateTime.Now){
                    _context.Auctions.Remove(auct);
                    _context.SaveChanges();
                }
                else if(auct.EndDate <= DateTime.Now){
                    EndAuction(auct.Id);
                }
            }
            return View();
        }
        [HttpGet]
        [Route("/New")]
        public IActionResult New()
        {
            ViewData["Error"] = "";   
            ViewBag.userFirstName = HttpContext.Session.GetString("userFirstName");
            if(ViewBag.userFirstName == null)
            {
                return Redirect("/");
            }
            
            return View("New");
        }
        [HttpPost]
        [Route("/NewAuction")]
        public IActionResult NewAuction(AuctionViewModel model)
        {
            if(ModelState.IsValid)
            {
                int? loggedIn = HttpContext.Session.GetInt32("userID");
                User loggedUserInstance = _context.Users.SingleOrDefault(user => user.Id == (int)loggedIn);
                DateTime dateValue = DateTime.Parse(model.EndDate);
                if( dateValue >= DateTime.Now){
                Auction AuctionInstance = new Auction
                {
                    Product = model.Product,
                    Description = model.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    HighestBid = model.HighestBid,
                    EndDate = dateValue,
                    TimeAgo = dateValue.Humanize(),
                    Author = HttpContext.Session.GetString("userFirstName")
                };
                _context.Auctions.Add(AuctionInstance);
                _context.SaveChanges();
                Auction thisOne = _context.Auctions.FirstOrDefault(auc => auc.Product == model.Product);
                return Redirect($"/Auction/{thisOne.Id}");
            }
            ViewData["Error"] = "Oops, please plan for the future";
            return View("New");
            }
            return View("New", model);
        }
        [HttpGet]
        [Route("/Auction/{AucId}")]
        public IActionResult SingleAuction(int AucId)
        {
            
            ViewBag.userFirstName = HttpContext.Session.GetString("userFirstName");
            ViewBag.userID = HttpContext.Session.GetInt32("userID");
            if(ViewBag.userFirstName == null)
            {
                return Redirect("/");
            }
            Auction AuctionInstance = _context.Auctions.SingleOrDefault(auc => auc.Id == AucId);
            int? UserId = HttpContext.Session.GetInt32("userID");
            ViewBag.Auction = AuctionInstance;
            return View("SingleAuction");
        }
        [HttpPost]
        [Route("/Bid")]
        public IActionResult Delete(int auctId, int bidAmount)
        {
            int sessUser = (int)HttpContext.Session.GetInt32("userID");
            Auction toBeBidOn = _context.Auctions.SingleOrDefault(auct => auct.Id == auctId);
            User biddingUser = _context.Users.SingleOrDefault(user => user.Id == sessUser);
            if(toBeBidOn.HighestBid < bidAmount && biddingUser.Wallet > bidAmount)
            {
                toBeBidOn.HighestBid = bidAmount;
                toBeBidOn.BidId = biddingUser.FirstName;
                _context.SaveChanges();
                return Redirect("/Main");
            }
            return Redirect($"/Auction/{auctId}");
        }
        [HttpPost]
        [Route("/Delete")]
        public IActionResult Delete(int auctId)
        {
            Auction toBeDeleted = _context.Auctions.SingleOrDefault(auct => auct.Id == auctId);
            _context.Auctions.Remove(toBeDeleted);
            _context.SaveChanges();
            return Redirect("/Main");
        }

    }
}
