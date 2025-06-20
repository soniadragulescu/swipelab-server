﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SwipeLab.Data.Postgres.Configuration;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250411205144_AddBioColumn")]
    partial class AddBioColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfile", b =>
                {
                    b.Property<Guid>("DatingProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<Guid?>("DatingProfileSetId")
                        .HasColumnType("uuid");

                    b.Property<int>("Drinking")
                        .HasColumnType("integer");

                    b.Property<int>("Education")
                        .HasColumnType("integer");

                    b.Property<int>("Ethnicity")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.PrimitiveCollection<List<string>>("Hobbies")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("KidsPreference")
                        .HasColumnType("integer");

                    b.Property<int>("LookingFor")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RowCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RowLastUpdated")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowStatus")
                        .HasColumnType("integer");

                    b.Property<int>("Smoking")
                        .HasColumnType("integer");

                    b.HasKey("DatingProfileId");

                    b.HasIndex("DatingProfileSetId");

                    b.ToTable("DatingProfiles");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfileFeedback.DatingProfileReflection", b =>
                {
                    b.Property<Guid>("DatingProfileReflectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("ChangedOpinion")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("DatingProfileId")
                        .HasColumnType("uuid");

                    b.Property<string>("PromptAnswers")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("RowCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RowLastUpdated")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowStatus")
                        .HasColumnType("integer");

                    b.HasKey("DatingProfileReflectionId");

                    b.HasIndex("DatingProfileId");

                    b.ToTable("DatingProfileReflections");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfileFeedback.DatingProfileSwipe", b =>
                {
                    b.Property<Guid>("DatingProfileSwipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DatingProfileId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RowCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RowLastUpdated")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowStatus")
                        .HasColumnType("integer");

                    b.Property<int>("SwipeState")
                        .HasColumnType("integer");

                    b.Property<int>("TimeSpentSeconds")
                        .HasColumnType("integer");

                    b.HasKey("DatingProfileSwipeId");

                    b.HasIndex("DatingProfileId");

                    b.ToTable("DatingProfileSwipes");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfileSet", b =>
                {
                    b.Property<Guid>("DatingProfileSetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RowCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RowLastUpdated")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowStatus")
                        .HasColumnType("integer");

                    b.HasKey("DatingProfileSetId");

                    b.ToTable("DatingProfileSets");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.Experiment", b =>
                {
                    b.Property<Guid>("ExperimentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DatingProfileSetId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RowCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RowLastUpdated")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowStatus")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("ExperimentId");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Models.Entities.Participant", b =>
                {
                    b.Property<Guid>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CountryOfResidency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Ethnicity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<int>("InterestedIn")
                        .HasColumnType("integer");

                    b.PrimitiveCollection<int[]>("KnownDatingApps")
                        .HasColumnType("integer[]");

                    b.Property<int>("MaxAge")
                        .HasColumnType("integer");

                    b.Property<int>("MinAge")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RowCreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RowLastUpdated")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowStatus")
                        .HasColumnType("integer");

                    b.Property<int>("UsageOfDatingApps")
                        .HasColumnType("integer");

                    b.HasKey("ParticipantId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfile", b =>
                {
                    b.HasOne("SwipeLab.Data.Postgres.Entities.DatingProfileSet", "DatingProfileSet")
                        .WithMany("DatingProfiles")
                        .HasForeignKey("DatingProfileSetId");

                    b.Navigation("DatingProfileSet");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfileFeedback.DatingProfileReflection", b =>
                {
                    b.HasOne("SwipeLab.Data.Postgres.Entities.DatingProfile", "DatingProfile")
                        .WithMany()
                        .HasForeignKey("DatingProfileId");

                    b.Navigation("DatingProfile");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfileFeedback.DatingProfileSwipe", b =>
                {
                    b.HasOne("SwipeLab.Data.Postgres.Entities.DatingProfile", "DatingProfile")
                        .WithMany()
                        .HasForeignKey("DatingProfileId");

                    b.Navigation("DatingProfile");
                });

            modelBuilder.Entity("SwipeLab.Data.Postgres.Entities.DatingProfileSet", b =>
                {
                    b.Navigation("DatingProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}
