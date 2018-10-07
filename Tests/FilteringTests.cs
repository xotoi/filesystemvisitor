using Shouldly;
using System;
using System.IO;
using Xunit;
using FileSystemVisitor;
using Moq;

namespace Tests
{
    public class FilteringTests
    {
        private Mock<FileSystemInfo> _fileSystemInfo;
        private int _delegatesCallCount;
        private FileSystemVisitor.FileSystemVisitor _visitor;
        private string _startPath;
        
        public FilteringTests()
        {
            _fileSystemInfo = new Mock<FileSystemInfo>();            
            _delegatesCallCount = 0;
            _startPath = "D://";

            _visitor = new FileSystemVisitor.FileSystemVisitor(_startPath, (info) => false);
        }

        [Fact]
        public void FilteredItemFindedCall()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            _visitor.FilterItemFinded(
            fileSystemInfo, info => true, (s, e) => _delegatesCallCount++, (s, e) => _delegatesCallCount++, OnEvent);

            // Assert
            _delegatesCallCount.ShouldBe(2);
        }

        [Fact]
        public void ItemNotPassFilter()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            _visitor.FilterItemFinded(
                fileSystemInfo, info => false, (s, e) => _delegatesCallCount++, (s, e) => _delegatesCallCount++, OnEvent);

            // Assert
            _delegatesCallCount.ShouldBe(1);
        }

        [Fact]
        public void ItemFinded_ContinueSearchResult()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            var result = _visitor.FilterItemFinded(fileSystemInfo, null, (s, e) => { }, null, OnEvent);

            // Assert
            result.ShouldBe(FilteringSteps.Continue);
        }

        [Fact]
        public void FilteredItemFinded_ContinueSearchResult()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            var result = _visitor.FilterItemFinded(
                fileSystemInfo, info => true, (s, e) => { }, (s, e) => { }, OnEvent);

            // Assert
            result.ShouldBe(FilteringSteps.Continue);
        }

        [Fact]
        public void FindedItemSkipped_SkipElementResult()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            var result = _visitor.FilterItemFinded(
                fileSystemInfo, info => true, (s, e) =>
                {
                    _delegatesCallCount++;
                    e.StepType = FilteringSteps.Skip;
                }, (s, e) => _delegatesCallCount++, OnEvent);

            // Assert
            result.ShouldBe(FilteringSteps.Skip);
            _delegatesCallCount.ShouldBe(1);
        }

        [Fact]
        public void FilteredFindedItemSkipped_SkipElementResult()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            var result = _visitor.FilterItemFinded(
                fileSystemInfo, info => true,
                (s, e) => _delegatesCallCount++,
                (s, e) =>
                {
                    _delegatesCallCount++;
                    e.StepType = FilteringSteps.Skip;
                }, OnEvent);

            // Assert
            result.ShouldBe(FilteringSteps.Skip);
            _delegatesCallCount.ShouldBe(2);
        }

        [Fact]
        public void FindedItemStopped_StopSearchResult()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            var result = _visitor.FilterItemFinded(
                fileSystemInfo, info => true, (s, e) =>
                {
                    _delegatesCallCount++;
                    e.StepType = FilteringSteps.Stop;
                }, (s, e) => _delegatesCallCount++, OnEvent);

            // Assert
            result.ShouldBe(FilteringSteps.Stop);
            _delegatesCallCount.ShouldBe(1);
        }

        [Fact]
        public void FilteredFindedItemStopped_StopSearchResult()
        {
            // Arrange
            var fileSystemInfo = _fileSystemInfo.Object;
            // Act 
            var result = _visitor.FilterItemFinded(
                fileSystemInfo, info => true,
                (s, e) => _delegatesCallCount++,
                (s, e) =>
                {
                    _delegatesCallCount++;
                    e.StepType = FilteringSteps.Stop;
                }, OnEvent);

            // Assert
            result.ShouldBe(FilteringSteps.Stop);
            _delegatesCallCount.ShouldBe(2);
        }

        private void OnEvent<TArgs>(EventHandler<TArgs> someEvent, TArgs args)
        {
            someEvent?.Invoke(this, args);
        }
    }
}
