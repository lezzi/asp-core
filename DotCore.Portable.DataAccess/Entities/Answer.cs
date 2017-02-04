using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace DotCore.Portable.DataAccess.Entities
{
    public class Answer
    {
        #region Properties, Indexers

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected internal set; }

        [Required]
        public string Description { get; protected internal set; }

        public DateTime CreatedOn { get; protected internal set; }

        public string UserId { get; protected internal set; }

        [Required]
        public User User { get; protected internal set; }

        public int QuestionId { get; protected internal set; }

        [Required]
        public Question Question { get; protected internal set; }

        #endregion

        #region Constructors

        protected internal Answer()
        {
        }

        /// <exception cref="ArgumentNullException"><paramref name="question"/> or <paramref name="description"/> or <paramref name="userId"/> is <see langword="null"/></exception>
        public Answer([NotNull] Question question, [NotNull] string description, [NotNull] string userId)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));
            if (description == null)
                throw new ArgumentNullException(nameof(description));
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            QuestionId = question.Id;
            Question = question;
            Description = description;
            UserId = userId;

            CreatedOn = DateTime.UtcNow;
        }

        /// <exception cref="ArgumentNullException"><paramref name="question"/> or <paramref name="description"/> or <paramref name="user"/> is <see langword="null"/></exception>
        public Answer(Question question, string description, [NotNull] User user) : this(question, description, user.Id)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User = user;
        }

        #endregion
    }
}