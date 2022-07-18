using AutoFixture;
using DoorAccessApplication.Core.Exceptions;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using DoorAccessApplication.Core.Services;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DoorAccessApplication.Tests.Services
{
    public class LockServiceTests
    {
        private readonly Mock<ILockRepository> _lockRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ILockHistoryRepository> _lockHistoryRepository;
        private readonly ILockService _lockService;
        private readonly Fixture _fixture;

        public LockServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _lockRepository = new Mock<ILockRepository>();
            _lockHistoryRepository = new Mock<ILockHistoryRepository>();
            _userRepository = new Mock<IUserRepository>();
            _lockService = new LockService(_lockRepository.Object, _userRepository.Object, _lockHistoryRepository.Object);
        }

        [Fact]
        public async Task CreateLockSuccessfully()
        {
            //Arrange
            var user = _fixture.Create<User>();
            var createLock = _fixture.Create<Lock>();

            _userRepository.Setup(r => r.GetAsync(user.Id))
                .ReturnsAsync(user);

            _lockRepository.Setup(r => r.IsExistAsync(createLock.UniqueIdentifier))
                .ReturnsAsync(false);

            //Act
            var lockTool = await _lockService.CreateAsync(createLock, user.Id);

            //Assert
            _lockRepository.Verify(m => m.CreateAsync(createLock), Times.Once);
        }

        [Fact]
        public async Task CreateChargeStationThrowsException()
        {
            //Arrange
            var user = _fixture.Create<User>();
            var createLock = _fixture.Create<Lock>();

            _userRepository.Setup(r => r.GetAsync(user.Id))
                .ReturnsAsync(user);

            _lockRepository.Setup(r => r.IsExistAsync(createLock.UniqueIdentifier))
                .ReturnsAsync(true);

            //Act
            Func<Task<Lock>> act = async () => await _lockService.CreateAsync(createLock, user.Id);

            //Assert
            await act.Should().ThrowAsync<EntityAddForbiddenException>()
                .WithMessage("Lock already registered.");
        }

    }
}
