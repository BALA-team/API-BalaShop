using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ShoppingAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ShoppingAPI : IShopping
    {
        shoppingDBEntities se = new shoppingDBEntities();



        public Guid Login(string login, string password)
        {
            var user = from u in se.User
                       where String.Equals(u.UserLogin, login) && String.Equals(u.UserPassword, password)
                       select u;
            if (user.Count() > 0)
                return user.Select(u => u.UserId).FirstOrDefault();
            else
                return Guid.Empty;
        }


        public bool Register(string login, string email, string password)
        {

            User newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserEmail = email,
                UserLogin = login,
                UserPassword = password
            };
            try
            {
                var user = (from u in se.User where u.UserLogin == newUser.UserLogin select u);
                var userE = (from u in se.User where u.UserEmail == newUser.UserEmail select u);

                if (user.Count() > 0 || userE.Count() > 0)
                    return false;
                se.User.Add(newUser);
                se.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ListUser> GetUserList(Guid userId)
        {
            List<ListUser> lu = new List<ListUser>();

            var list = from ul in se.UserList
                       join sl in se.ShopppingList on ul.ListId equals sl.ListId
                       where ul.UserId == userId
                       select new { ShoppingL = sl, UserL = ul };
            foreach (var item in list)
            {
                ListUser listu = new ListUser();
                listu.ListId = item.ShoppingL.ListId;
                listu.ListName = item.ShoppingL.ListName;
                lu.Add(listu);
            }
            return lu;
        }

        public bool CreateNewList(string ListName, Guid userId)
        {
            Guid Id = Guid.NewGuid();
            ShopppingList sl = new ShopppingList
            {
                ListId = Id,
                ListName = ListName
            };
            Guid userListId = Guid.NewGuid();
            UserList ul = new UserList
            {
                Id = userListId,
                UserId = userId,
                ListId = Id


            };
            try
            {
                se.ShopppingList.Add(sl);
                se.UserList.Add(ul);
                se.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool AddProductToList(string ProductName, Guid ListId)
        {
            Guid id = Guid.NewGuid();
            ShoppingProduct lp = new ShoppingProduct
            {
                ProductId = id,
                ProductName = ProductName,
                ListId = ListId,
                IsBought = false
            };
            try
            {
                se.ShoppingProduct.Add(lp);
                se.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<ListProduct> GetProductsFromList(Guid ListId)
        {

            List<ListProduct> list = new List<ListProduct>();
            var product = from pr in se.ShoppingProduct where pr.ListId == ListId select pr;
            foreach (var item in product)
            {
                ListProduct lp = new ListProduct();
                lp.ProductId = item.ProductId;
                lp.ProductName = item.ProductName;
                lp.IsBought = item.IsBought;
                lp.ListId = item.ListId;
                list.Add(lp);
            }
            return list;

        }

        public bool Sync(Dictionary<Guid, bool> products, Guid listId)
        {
            var list = from sp in se.ShoppingProduct where sp.ListId == listId select sp;
            List<ShoppingProduct> p = new List<ShoppingProduct>();
            foreach (var item in list)
            {
                foreach (var i in products)
                {
                    if (item.ProductId == i.Key)
                    {
                        item.IsBought = i.Value;

                        ShoppingProduct prod = new ShoppingProduct
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            IsBought = item.IsBought,
                            ListId = item.ListId
                        };


                    }

                }

            }
            try
            {
                se.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }




        }

        public bool Share(Guid userId, Guid listId)
        {
            Guid id = Guid.NewGuid();
            UserList ul = new UserList
            {
                Id = id,
                ListId = listId,
                UserId = userId,
            };
            try
            {
                se.UserList.Add(ul);
                se.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Guid GetUserId(string userName)
        {
            var user = (from u in se.User where u.UserLogin == userName select u.UserId).FirstOrDefault();
            return Guid.Parse(user.ToString());
        }

        public bool CheckLoginIsExist(string userName)
        {
            var user = (from u in se.User where u.UserLogin == userName select u);
            if (user.Count() > 0)
                return true;
            else
                return false;
        }

        public bool CheckEmailIsExist(string email)
        {
            var user = (from u in se.User where u.UserEmail == email select u);
            if (user.Count() > 0)
                return true;
            else
                return false;
        }

        public bool DeleteList(Guid listId, Guid userId)
        {
            var list = new UserList();
            list = (from l in se.UserList where l.ListId == listId && l.UserId == userId select l).FirstOrDefault();
            try
            {
                se.UserList.Remove(list);
                se.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }



        }
    }
}
