using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RetroShop_Server.Repository;

namespace RetroShop_Server.Services
{
    public class UserService : UserSrv.UserSrvBase
    {
        private readonly UserContext _context;
        public UserService(UserContext context)
        {
            _context = context;
        }
        public override Task<Users> GetAll(Empty request, ServerCallContext context)
        {
            var response = new Users();
            var users = (from u in _context.User
                         select new User
                         {
                             Name = u.Name,
                             SurName = u.SurName,
                             Dni = u.Dni,
                             Mail = u.Mail,
                             UserName = u.UserName,
                             Password = u.Password
                         }
            ).ToArray();
            response.Items.AddRange(users);
            return Task.FromResult(response);
        }

        public override Task<User> GetByUserName(UserNameFilter request, ServerCallContext context)
        {
            var user = _context.User.FirstOrDefault(u => u.UserName == request.UserName);
            var sUser = new User
            {
                Name = user.Name,
                SurName = user.SurName,
                Dni = user.Dni,
                Mail = user.Mail,
                UserName = user.UserName,
                Password = user.Password
            };
            return Task.FromResult(sUser);
        }
        public override Task<User> Post (User request, ServerCallContext context)
        {
            var user = new Model.User()
            {
                Name = request.Name,
                SurName = request.SurName,
                Dni = request.Dni,
                Mail = request.Mail,
                UserName = request.UserName,
                Password = request.Password
            };

            var response = _context.User.Add(user);
            _context.SaveChangesAsync();

            var resUser = new User()
            {
                Name = response.Entity.Name,
                SurName = response.Entity.SurName,
                Dni = response.Entity.Dni,
                Mail = response.Entity.Mail,
                UserName = response.Entity.UserName,
                Password = response.Entity.Password
            };
            return Task.FromResult(resUser);
        }
        public override Task<User> Put(User request, ServerCallContext context)
        {
            var user = _context.User.FirstOrDefault(u => u.UserName == request.UserName);
            if(user == null)
                return Task.FromResult<User>(null);
            user.Name = request.Name;
            user.SurName = request.SurName;
            user.Mail = request.Mail;
            user.Dni = request.Dni;
            user.UserName = request.UserName;
            user.Password = request.Password;
            var sUser = new User
            {
                Name = user.Name,
                SurName = user.SurName,
                Dni = user.Dni,
                Mail = user.Mail,
                UserName = user.UserName,
                Password = user.Password
            };
            return Task.FromResult(sUser);
        }
        public override Task<Empty> Delete(UserNameFilter request, ServerCallContext context)
        {
            var user = _context.User.FirstOrDefault(u => u.UserName == request.UserName);
            if (user == null)
                return Task.FromResult<Empty>(null);
            _context.Remove(user);
            _context.SaveChangesAsync();
            return Task.FromResult<Empty>(new Empty());
        }
    }
}
