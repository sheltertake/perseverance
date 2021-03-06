using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using Perseverance;

namespace PerseveranceUnitTests
{
    /// <summary>
    /// # Mars Rover
    ///  You're part of the team that explores Mars by sending remotely controlled vehicles to the surface of the planet.
    ///  Develop an API that translates the commands sent from earth to instructions that are understood by the rover.
    /// 
    ///  Requirements
    ///  - You are given the initial starting point (x, y) of a rover and the direction (N, S, E, W) it is facing.
    ///  - The rover receives a character array of commands.
    ///  - Implement commands that move the rover forward/backward (f, b).
    ///  - Implement commands that turn the rover left/right (l, r).
    ///  - Implement wrapping from one edge of the grid to another. (planets are spheres after all)
    ///  - Implement obstacle detection before each move to a new square.If a given sequence of commands encounters an obstacle, the rover moves up to the last possible point, aborts the sequence and reports the obstacle.
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoverTests
    {
        /// <summary>
        /// Test move forward twice with commands FF
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// 
        /// final state: 
        /// | R |   |   |
        /// |   |   |   |
        /// |   |   |   |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveForward()
        {

            var planet = new Planet(2, 2);
            var rover = new Rover(planet);
            rover.Move("F");
            rover.Y.Should().Be(1);
            rover.Move("F");
            rover.Y.Should().Be(2);
        }

        /// <summary>
        /// Test move backward twice with commands BB
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// | R |   |   |
        /// |   |   |   |
        /// |   |   |   |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveBackward()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 0, 2);
            rover.Move("B");
            rover.Y.Should().Be(1);
            rover.Move("B");
            rover.Y.Should().Be(0);
        }

        /// <summary>
        /// Test move right twice with commands RR
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   |   | R |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveRight()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 0, 0);
            rover.Move("R");

            rover.X.Should().Be(1);
            rover.Move("R");
            rover.X.Should().Be(2);
        }

        /// <summary>
        /// Test move left twice with commands LL
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   |   | R |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveLeft()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 2, 0);
            rover.Move("L");
            rover.X.Should().Be(1);
            rover.Move("L");
            rover.X.Should().Be(0);
        }

        /// <summary>
        /// Test move forward and wrap planet with commands FFF
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveForwardAndWrap()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet);
            rover.Move("F");
            rover.Y.Should().Be(1);
            rover.Move("F");
            rover.Y.Should().Be(2);
            rover.Move("F");
            rover.Y.Should().Be(0);
        }

        /// <summary>
        /// Test move backward and wrap planet with commands BBB
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   |   | R |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   |   | R |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveBackwardAndWrap()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 0, 2);
            rover.Move("B");
            rover.Y.Should().Be(1);
            rover.Move("B");
            rover.Y.Should().Be(0);
            rover.Move("B");
            rover.Y.Should().Be(2);
        }

        /// <summary>
        /// Test move right and wrap planet with commands RRR
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveRightAndWrap()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 0, 0);
            rover.Move("R");

            rover.X.Should().Be(1);
            rover.Move("R");
            rover.X.Should().Be(2);
            rover.Move("R");
            rover.Y.Should().Be(0);
        }

        /// <summary>
        /// Test move left and wrap planet with commands LLL
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   |   | R |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   |   | R |
        /// </remarks>
        [Test]
        public void RoverShouldKnowHowMoveLeftAndWrap()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 2, 0);
            rover.Move("L");
            rover.X.Should().Be(1);
            rover.Move("L");
            rover.X.Should().Be(0);
            rover.Move("L");
            rover.X.Should().Be(2);
        }

        /// <summary>
        /// Test command loop thinking obstacles.
        /// At first unknown char (or obstacle) rover will stop
        /// First test FFF then test FFTF. First commands should success. Second one not. 
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// 
        /// final state: 
        /// | R |   |   |
        /// |   |   |   |
        /// |   |   |   |
        /// </remarks>
        [Test]
        public void RoverShouldStopAtFirstUnknownCommand()
        {
            var planet = new Planet(2, 2);
            var rover = new Rover(planet, 0, 0);
            rover.Y.Should().Be(0);// X = 0
            rover.Move("FFF");// original point
            rover.Y.Should().Be(0);// X = 0
            rover.Move("FFTF");// not original point
            rover.Y.Should().Be(2);
        }

        /// <summary>
        /// Obstacle Ahead
        /// First obstacle put in the map.
        /// Rover should move only first F
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// | O |   |   |
        /// |   |   |   |
        /// | R |   |   |
        /// 
        /// final state: 
        /// | O |   |   |
        /// | R |   |   |
        /// |   |   |   |
        /// </remarks>
        [Test]
        public void RoverCantMoveForwardThroughObstacle()
        {
            var obstacles = new[] { new Point { X = 0, Y = 2 } };
            var planet = new Planet(2, 2, obstacles);
            var rover = new Rover(planet, 0, 0);
            rover.Move("F");
            rover.Y.Should().Be(1);
            rover.Move("F");// not original point
            rover.Y.Should().Be(1);
        }

        /// <summary>
        /// Obstacle behind
        /// Rover should move only first B
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// | R |   |   |
        /// |   |   |   |
        /// | O |   |   |
        /// 
        /// final state: 
        /// |   |   |   |
        /// | R |   |   |
        /// | O |   |   |
        /// </remarks>
        [Test]
        public void RoverCantMoveBackwardThroughObstacle()
        {
            var obstacles = new[] { new Point { X = 0, Y = 0 } };
            var planet = new Planet(2, 2, obstacles);
            var rover = new Rover(planet, 0, 2);
            rover.Move("B");
            rover.Y.Should().Be(1);
            rover.Move("B");// not original point
            rover.Y.Should().Be(1);
        }

        /// <summary>
        /// Obstacle on the right
        /// Rover should move only first R
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | R |   | O |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// |   | R | O |
        /// </remarks>
        [Test]
        public void RoverCantMoveRightThroughObstacle()
        {
            var obstacles = new[] { new Point { X = 2, Y = 0 } };
            var planet = new Planet(2, 2, obstacles);
            var rover = new Rover(planet, 0, 0);
            rover.Move("R");
            rover.X.Should().Be(1);
            rover.Move("R");// not original point
            rover.X.Should().Be(1);
        }

        /// <summary>
        /// Obstacle on the left
        /// Rover should move only first L
        /// </summary>
        /// <remarks>
        /// initial state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | O |   | R |
        /// 
        /// final state: 
        /// |   |   |   |
        /// |   |   |   |
        /// | O | R |   |
        /// </remarks>
        [Test]
        public void RoverCantMoveLeftThroughObstacle()
        {
            var obstacles = new[] { new Point { X = 0, Y = 0 } };
            var planet = new Planet(2, 2, obstacles);
            var rover = new Rover(planet, 2, 0);
            rover.Move("L");
            rover.X.Should().Be(1);
            rover.Move("L");// not original point
            rover.X.Should().Be(1);
        }

        /// <summary>
        /// Rover should land on planet
        /// </summary>
        /// <remarks>
        /// land attempts: 
        ///   *   *   *
        /// |   |   |   | *
        /// |   |   |   | *
        /// |   |   |   | *
        /// </remarks>
        [Test]
        public void RoverShouldLandInPlanetOtherwiseExplodes([Range(0, 2)] byte inbound)
        {
            var planet = new Planet(2, 2);
            Action attemptLandYOverflow = () => _ = new Rover(planet, inbound, 3);
            attemptLandYOverflow.Should().Throw<ArgumentException>();
            Action attemptLandXOverflow = () => _ = new Rover(planet, 3, inbound);
            attemptLandXOverflow.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// Rover should not land over obstacle
        /// </summary>
        /// <remarks>
        /// land attempts: 
        /// | R | R | R | 
        /// | R | O | R | 
        /// | R | R | R | 
        /// </remarks>
        [Test]
        public void RoverShouldNotLandOverObstaclePlanetOtherwiseExplodes([Range(0, 2)] byte x, [Range(0, 2)] byte y)
        {
            var obstacles = new[] { new Point { X = 1, Y = 1 } };
            var planet = new Planet(2, 2, obstacles);

            // rover 1 explodes
            if (x == 1 && y == 1)
            {
                Action attemptLandOverObstacle = () => _ = new Rover(planet, x, y);
                attemptLandOverObstacle.Should().Throw<ArgumentException>();
            }
            else
            {
                // rover 2 not
                var rover = new Rover(planet, x, y);
                rover.X.Should().Be(x);
                rover.Y.Should().Be(y);
            }
        }
    }

}