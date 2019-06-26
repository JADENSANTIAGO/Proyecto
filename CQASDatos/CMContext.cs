using System;
using CQASEntidades.Negocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CQASDatos
{
    public partial class CMContext : DbContext
    {
        //public CMContext()
        //{
        //}

        public CMContext(DbContextOptions<CMContext> options)
            : base(options)
        {
        }
        public virtual DbSet<CqasAsignacionConsultorio> CqasAsignacionConsultorio { get; set; }
        public virtual DbSet<CqasCita> CqasCita { get; set; }
        public virtual DbSet<CqasConsultorio> CqasConsultorio { get; set; }
        public virtual DbSet<CqasConsultorioHorario> CqasConsultorioHorario { get; set; }
        public virtual DbSet<CqasEspecialidad> CqasEspecialidad { get; set; }
        public virtual DbSet<CqasEstadoCivil> CqasEstadoCivil { get; set; }
        public virtual DbSet<CqasEtnia> CqasEtnia { get; set; }
        public virtual DbSet<CqasGenero> CqasGenero { get; set; }
        public virtual DbSet<CqasHistoriaClinica> CqasHistoriaClinica { get; set; }
        public virtual DbSet<CqasMedico> CqasMedico { get; set; }
        public virtual DbSet<CqasMedicoCitas> CqasMedicoCitas { get; set; }
        public virtual DbSet<CqasMedicoEspecialidad> CqasMedicoEspecialidad { get; set; }
        public virtual DbSet<CqasMenu> CqasMenu { get; set; }
        public virtual DbSet<CqasMenuPerfil> CqasMenuPerfil { get; set; }
        public virtual DbSet<CqasPaciente> CqasPaciente { get; set; }
        public virtual DbSet<CqasPerfil> CqasPerfil { get; set; }
        public virtual DbSet<CqasPersona> CqasPersona { get; set; }
        public virtual DbSet<CqasSexo> CqasSexo { get; set; }
        public virtual DbSet<CqasTipoSangre> CqasTipoSangre { get; set; }
        public virtual DbSet<CqasUsuario> CqasUsuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CqasAsignacionConsultorio>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_AsignacionConsultorio");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoConsultorioNavigation)
                    .WithMany(p => p.CqasAsignacionConsultorio)
                    .HasForeignKey(d => d.CodigoConsultorio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQASIGNACIONCONSULTORIO_ASCQCONSULTORIO");

                entity.HasOne(d => d.CodigoMedicoEspecialidadNavigation)
                    .WithMany(p => p.CqasAsignacionConsultorio)
                    .HasForeignKey(d => d.CodigoMedicoEspecialidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQASIGNACIONCONSULTORIO_ASCQMEDICOESPECIALIDAD");
            });

            modelBuilder.Entity<CqasCita>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Cita");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.CodigoConsultorioHorarioNavigation)
                    .WithMany(p => p.CqasCita)
                    .HasForeignKey(d => d.CodigoConsultorioHorario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQCITA_ASCQ_CONSULTORIOHORARIO");

                entity.HasOne(d => d.CodigoPacienteNavigation)
                    .WithMany(p => p.CqasCita)
                    .HasForeignKey(d => d.CodigoPaciente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQ_CITA_ASCQPACIENTE");
            });

            modelBuilder.Entity<CqasConsultorio>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Consultorio");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasConsultorioHorario>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_ConsultorioHorario");

                entity.Property(e => e.Dia)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoConsultorioAsignadoNavigation)
                    .WithMany(p => p.CqasConsultorioHorario)
                    .HasForeignKey(d => d.CodigoConsultorioAsignado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQ_CONSULTORIOHORARIO_ASCQASIGNACIONCONSULTORIO");
            });

            modelBuilder.Entity<CqasEspecialidad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Especialidad");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasEstadoCivil>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_EstadoCivil");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasEtnia>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Etnia");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasGenero>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Genero");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasHistoriaClinica>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_HistoriaClinica");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Reseta)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.CodigoCitaNavigation)
                    .WithMany(p => p.CqasHistoriaClinica)
                    .HasForeignKey(d => d.CodigoCita)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQHISTORIACLINICA_ASCQCITA");
            });

            modelBuilder.Entity<CqasMedico>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Medico");

                entity.HasOne(d => d.CodigoPersonaNavigation)
                    .WithMany(p => p.CqasMedico)
                    .HasForeignKey(d => d.CodigoPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQMEDICO_ASCQPERSONA");
            });

            modelBuilder.Entity<CqasMedicoCitas>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_MedicoCitas");

                entity.Property(e => e.FechaFin).HasColumnType("date");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.HasOne(d => d.CodigoAsignacionConsultorioNavigation)
                    .WithMany(p => p.CqasMedicoCitas)
                    .HasForeignKey(d => d.CodigoAsignacionConsultorio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQMEDICOCITAS_ASCQASIGNACIONCONSULTORIO");
            });

            modelBuilder.Entity<CqasMedicoEspecialidad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_MedicoEspecialidad");

                entity.HasOne(d => d.CodigoEspecialidadNavigation)
                    .WithMany(p => p.CqasMedicoEspecialidad)
                    .HasForeignKey(d => d.CodigoEspecialidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQMEDICOESPECIALIDAD_ASCQESPECIALIDAD");

                entity.HasOne(d => d.CodigoMedicoNavigation)
                    .WithMany(p => p.CqasMedicoEspecialidad)
                    .HasForeignKey(d => d.CodigoMedico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQMEDICOESPECIALIDAD_ASCQMEDICO");
            });

            modelBuilder.Entity<CqasMenu>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Menu");

                entity.Property(e => e.Cabezera)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasMenuPerfil>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_MenuPerfil");

                entity.HasOne(d => d.CodigoMenuNavigation)
                    .WithMany(p => p.CqasMenuPerfil)
                    .HasForeignKey(d => d.CodigoMenu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQMENUPERFIL_ASCQMENU");

                entity.HasOne(d => d.CodigoPerfilNavigation)
                    .WithMany(p => p.CqasMenuPerfil)
                    .HasForeignKey(d => d.CodigoPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQMENUPERFIL_ASCQPERFIL");
            });

            modelBuilder.Entity<CqasPaciente>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Paciente");

                entity.Property(e => e.LugarNacimiento)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LugarRecidencia)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoEtniaNavigation)
                    .WithMany(p => p.CqasPaciente)
                    .HasForeignKey(d => d.CodigoEtnia)
                    .HasConstraintName("FK_CQAS_Paciente_CQAS_Etnia");

                entity.HasOne(d => d.CodigoPersonaNavigation)
                    .WithMany(p => p.CqasPaciente)
                    .HasForeignKey(d => d.CodigoPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQPACIENTE_ASCQPERSONA");

                entity.HasOne(d => d.CodigoSexoNavigation)
                    .WithMany(p => p.CqasPaciente)
                    .HasForeignKey(d => d.CodigoSexo)
                    .HasConstraintName("FK_CQAS_Paciente_CQAS_Sexo");

                entity.HasOne(d => d.CodigoTipoSangreNavigation)
                    .WithMany(p => p.CqasPaciente)
                    .HasForeignKey(d => d.CodigoTipoSangre)
                    .HasConstraintName("FK_CQAS_Paciente_CQAS_TipoSangre");
            });

            modelBuilder.Entity<CqasPerfil>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Perfil");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasPersona>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Persona");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Nacionalidad)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoEstadoCivilNavigation)
                    .WithMany(p => p.CqasPersona)
                    .HasForeignKey(d => d.CodigoEstadoCivil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQPERSONA_ASCQESTADOCIVIL");

                entity.HasOne(d => d.CodigoGeneroNavigation)
                    .WithMany(p => p.CqasPersona)
                    .HasForeignKey(d => d.CodigoGenero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQPERSONA_ASCQGENERO");
            });

            modelBuilder.Entity<CqasSexo>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Sexo");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasTipoSangre>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_TipoSangre");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CqasUsuario>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("CQAS_Usuario");

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoPerfilNavigation)
                    .WithMany(p => p.CqasUsuario)
                    .HasForeignKey(d => d.CodigoPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQUSUARIO_ASCQPERFIL");

                entity.HasOne(d => d.CodigoPersonaNavigation)
                    .WithMany(p => p.CqasUsuario)
                    .HasForeignKey(d => d.CodigoPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASCQUSUARIO_ASCQPERSONA");
            });
        }
    }
}
