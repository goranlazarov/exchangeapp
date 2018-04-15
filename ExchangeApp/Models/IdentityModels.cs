using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Text;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure.Interception;
using System.Web;
using System.Data.Common;


namespace ExchangeApp.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Required Field: First name")]
        [Display(Name = "First name")]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required Field: Last name")]
        [Display(Name = "Last name")]
        [StringLength(40)]
        public string LastName { get; set; }

        public int? IdLogin { get; set; }

        public string Roles_keys { get; set; }

        [Display(Name = "Date of password")]
        public DateTime? Date_pwd { get; set; }

        [Display(Name = "Date locked")]
        public DateTime? Date_lock { get; set; }

        [NotMapped]
        public virtual String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Timestamp]
        public byte[] LockStamp { get; set; }


        //^                         Start anchor
        //(?=.*[A-Z].*[A-Z])        Ensure string has two uppercase letters.
        //(?=.*[!@#$&*])            Ensure string has one special case letter.
        //(?=.*[0-9].*[0-9])        Ensure string has two digits.
        //(?=.*[a-z].*[a-z].*[a-z]) Ensure string has three lowercase letters.
        //.{8}                      Ensure string is of length 8.
        //$                         End anchor.

        [NotMapped]
        [RegularExpression(@"^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}$", ErrorMessage = "Password must contain at least 8 characters of which 2 uppercase letters, 1 special case letter, 2 digits, 3 lowercase letters.")]
        [StringLength(20)]
        public string Password { get; set; }

        [NotMapped]
        [StringLength(20)]
        public string OldPassword { get; set; }

        [NotMapped]
        [CompareAttribute("Password")]
        [StringLength(20)]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        [Display(Name = "Roles values")]
        public string[] Roles_values { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 300;

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<NomDegreeLevel> DegreeLevels { get; set; }

        public DbSet<NomTypeOfExchange> TypesOfExchange { get; set; }

        public DbSet<NomApplicantHighestDegree> ApplicantHighestDegrees { get; set; }

        public DbSet<NomCountry> Countries { get; set; }

        public DbSet<NomEnglishLevel> EnglishLevels { get; set; }

        public DbSet<NomNationality> Nationalities { get; set; }

        public DbSet<NomRegion> Regions { get; set; }

        public DbSet<NomSchoolYear> SchoolYears { get; set; }

        public DbSet<Semester> Semesters { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<FacultyCourses> FacultyCourses { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Faculty>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.FacultyObj)
                .HasForeignKey(e => e.FacultyId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<NomApplicantHighestDegree>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomCountry>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomCountry>()
                .HasMany(e => e.Faculties)
                .WithRequired(e => e.CountryObj)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NomDegreeLevel>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomEnglishLevel>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomNationality>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomRegion>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomRegion>()
                .HasMany(e => e.Countries)
                .WithRequired(r=>r.RegionObj)
                .HasForeignKey(e => e.RegionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NomSchoolYear>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomTypeOfExchange>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<NomTypeOfExchange>()
                .HasMany(e => e.StudentFaculties)
                .WithOptional(e => e.StudentTypeOfExchangeObj)
                .HasForeignKey(e => e.StudentTypeOfExchangeId);

            modelBuilder.Entity<NomTypeOfExchange>()
                .HasMany(e => e.TeacherFaculties)
                .WithOptional(e => e.FacultyTypeOfExchangeObj)
                .HasForeignKey(e => e.FacultyTypeOfExchangeId);

            modelBuilder.Entity<Semester>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Subject>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            try
            {
                foreach (var entity in ChangeTracker.Entries<BaseObject>())
                    if (entity.State == EntityState.Modified)
                        entity.Entity.Access();
                return base.SaveChanges();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new Exception(sb.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, please reload it.");
            }
            catch (DbUpdateException dbUpdateEx)
            {
                if (dbUpdateEx != null
                && dbUpdateEx.InnerException != null
                && dbUpdateEx.InnerException.InnerException != null)
                {
                    SqlException sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
                    if (sqlException != null)
                    {
                        switch (sqlException.Number)
                        {
                            case 229:
                                throw new Exception("You don't have the required permissons to perform this action!");
                            case 547:   // Constraint check violation
                                throw new Exception("This record cannot be deleted because it is referenced in another record!");
                            case 2627:  // Unique constraint error
                            case 2601:  // Duplicated key row error
                                // Constraint violation exception
                                throw new Exception("This record already exists in the database!");

                            default:
                                // A custom exception of yours for other DB issues
                                throw new Exception(dbUpdateEx.Message, dbUpdateEx.InnerException);
                        }
                    }
                }
                throw new Exception(dbUpdateEx.Message, dbUpdateEx.InnerException);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new Exception("Record cannot be deleted because it is referenced in another record.");
                }
                else
                    throw new Exception(ex.Message);
            }
        }
    }

    public class CommandInterceptor : DbCommandInterceptor
    {
        private string ExecAsUser()
        {
            string username = String.Empty;

            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                username = HttpContext.Current.User.Identity.Name;
            else
                username = "login_user";

            return String.Format(@"exec as user='{0}'", username);
        }

        private void AddExecAsUserToCommand(DbCommand command)
        {
            command.CommandText = String.Format("{0} {1} {2}", ExecAsUser(), command.CommandText, "revert");
        }


        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            AddExecAsUserToCommand(command);
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            AddExecAsUserToCommand(command);
            base.ScalarExecuting(command, interceptionContext);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            AddExecAsUserToCommand(command);
            base.ReaderExecuting(command, interceptionContext);
        }

        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuted(command, interceptionContext);
        }
    }

    public class CustomCompareAttribute : CompareAttribute
    {
        public CustomCompareAttribute(string otherProperty, string ErrorCode)
            : base(otherProperty)
        {
            ErrorMessage = CustomMessage(ErrorCode);
        }

        private static string CustomMessage(string ErrorCode)
        {
            string ErrorMsg = ErrorCode;
            //ErrorMsg = Translate.Item(ErrorCode);  <-- My logic from database
            return ErrorMsg;
        }

    }
}