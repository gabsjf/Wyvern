using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Wyvern.Domain.Entities;

namespace Wyvern.Infrastructure.Data
{
    public class WyvernDbContext : DbContext
    {
        public WyvernDbContext(DbContextOptions<WyvernDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sessao> Sessoes { get; set; }

        public DbSet<PersonagemPlayer> PersonagemPlayers { get; set; }
        public DbSet<PersonagemPericia> PersonagemPericias { get; set; }
        public DbSet<PersonagemMagia> PersonagemMagias { get; set; }
        public DbSet<PersonagemItem> PersonagemItens { get; set; }
        public DbSet<PersonagemCombate> PersonagemCombates { get; set; }
        public DbSet<Personagem> Personagens { get; set; }
        public DbSet<Pericia> Pericias { get; set; }
        public DbSet<Magia> Magias { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<Campanha> Campanhas { get; set; }
        public DbSet<Atributo> Atributos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<PersonagemItem>()
                .HasKey(pi => new { pi.PersonagemId, pi.ItemId });

            modelBuilder.Entity<PersonagemMagia>()
                .HasKey(pm => new { pm.PersonagemId, pm.MagiaId });

            modelBuilder.Entity<PersonagemPericia>()
                .HasKey(pp => new { pp.PersonagemId, pp.PericiaId });


            

            modelBuilder.Entity<Atributo>()
                .HasKey(a => a.PersonagemId);
            modelBuilder.Entity<Atributo>()
                .HasOne(a => a.Personagem)
                .WithOne(p => p.Atributo)
                .HasForeignKey<Atributo>(a => a.PersonagemId);

            modelBuilder.Entity<PersonagemCombate>()
                .HasKey(pc => pc.PersonagemId);
            modelBuilder.Entity<PersonagemCombate>()
                .HasOne(pc => pc.Personagem)
                .WithOne()
                .HasForeignKey<PersonagemCombate>(pc => pc.PersonagemId);

            modelBuilder.Entity<PersonagemPlayer>()
                .HasKey(pp => pp.PersonagemId);
            modelBuilder.Entity<PersonagemPlayer>()
                .HasOne(pp => pp.personagem) 
                .WithOne()
                .HasForeignKey<PersonagemPlayer>(pp => pp.PersonagemId);


          

            modelBuilder.Entity<Personagem>()
                .HasOne(p => p.CriadoPor)
                .WithMany()
                .HasForeignKey(p => p.CriadoPorId)
                .OnDelete(DeleteBehavior.Restrict); // O usuário é deletado, o personagem fica (ou você apaga na mão)

            modelBuilder.Entity<Campanha>()
                .HasOne(c => c.Mestre)
                .WithMany(u => u.Campanhas)
                .HasForeignKey(c => c.MestreId)
                .OnDelete(DeleteBehavior.Restrict);


            // 4. Mapeamento Manual de Nomes de Tabela
            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            base.OnModelCreating(modelBuilder);
        }


    }
}