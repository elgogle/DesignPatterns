/*

## 门面模式的定义

要求一个子系统的外部与其内部的通信必须通过一个统一的对象进行。门面模式提供一个高层次的接口，使得子系统更易于使用。
门面模式注重“统一的对象”，也就是提供一个访问子系统的接口，除了这个接口不允许有任何访问子系统的行为发生

*/

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignPatterns.Structural
{
    public class Facade : DesignPattern
    {
        public override void Execute()
        {
            var eventSystem = new EventSystem();

            IReadOnlyCollection<Event> events = eventSystem.FindEventsForDate(new DateTime(2018, 1, 3));
            Assert.Empty(events);

            events = eventSystem.FindEventsForDate(new DateTime(2018, 1, 10));
            Assert.NotEmpty(events);
            Assert.Equal(2, events.Count);

            Event firstEvent = events.FirstOrDefault();
            Assert.NotNull(firstEvent);
            Assert.Equal(EventType.Sport, firstEvent.EventType);
            Assert.Equal("Canadien vs Mapple Leafs", firstEvent.Title);
            Assert.Equal(new DateTime(2018, 1, 10, 12, 0, 0), firstEvent.EndsOn);
            
            Event secondEvent = events.ElementAtOrDefault(1);
            Assert.NotNull(secondEvent);
            Assert.Equal(EventType.Music, secondEvent.EventType);
            Assert.Equal("Bon Jovi", secondEvent.Title);
            Assert.Equal(new DateTime(2018, 1, 10, 22, 0, 0), secondEvent.EndsOn);
        }

        /* Facade */
        /// <summary>
        /// 门面，隐藏了子系统细节
        /// </summary>
        public class EventSystem
        {
            private readonly SportEventSystem _sportEventSystem;
            private readonly MusicEventSystem _musicEventSystem;

            public EventSystem()
            {
                this._sportEventSystem = new SportEventSystem();
                this._musicEventSystem = new MusicEventSystem();
            }

            public IReadOnlyCollection<Event> FindEventsForDate(DateTime date)
            {
                return this._sportEventSystem.GetSportEventsForDate(date).Select(Map)
                           .Concat(this._musicEventSystem.GetTickets(date).Select(Map))
                           .OrderBy(ticket => ticket.StartsOn)
                           .ToArray();
            }

            private static Event Map(SportEvent sportEvent)
            {
                return new Event(sportEvent.MatchName, EventType.Sport, sportEvent.StartDate, sportEvent.StartDate + sportEvent.Duration);
            }

            private static Event Map(MusicEvent musicEvent)
            {
                return new Event(musicEvent.Band, EventType.Music, musicEvent.Start, musicEvent.End);
            }
        }

        public class Event
        {
            public Event(string title, EventType type, DateTime startsOn, DateTime endsOn)
            {
                this.Title = title;
                this.EventType = type;
                this.StartsOn = startsOn;
                this.EndsOn = endsOn;
            }

            public string Title { get; }
            public DateTime StartsOn { get; }
            public DateTime EndsOn { get; }
            public EventType EventType { get; }
        }

        public enum EventType
        {
            Sport,
            Music
        }

        #region SubSystems

        public class SportEventSystem
        {
            private readonly SportEvent[] _sportEvents;

            public SportEventSystem()
            {
                this._sportEvents = new []
                {
                    new SportEvent("Canadien vs Mapple Leafs", new DateTime(2018, 1, 10, 10, 0, 0), TimeSpan.FromHours(2)),
                    new SportEvent("Broncos vs Texans", new DateTime(2018, 1, 12, 16, 0, 0), TimeSpan.FromHours(3))
                };
            }

            public SportEvent[] GetSportEventsForDate(DateTime date)
            {
                return this._sportEvents.Where(ticket => ticket.StartDate.Date == date.Date).ToArray();
            }
        }

        public class SportEvent
        {

            public SportEvent(string matchName, DateTime startDate, TimeSpan duration)
            {
                this.MatchName = matchName;
                this.StartDate = startDate;
                this.Duration = duration;
            }

            public string MatchName { get; }
            public DateTime StartDate { get; }
            public TimeSpan Duration { get; }
        }


        public class MusicEventSystem
        {
            private readonly MusicEvent[] _musicEvents;

            public MusicEventSystem()
            {
                this._musicEvents = new []
                {
                    new MusicEvent("Celine Dion", new DateTime(2018, 1, 6, 18, 0, 0), new DateTime(2018, 1, 10, 22, 0, 0)),
                    new MusicEvent("Bon Jovi", new DateTime(2018, 1, 10, 18, 0, 0), new DateTime(2018, 1, 10, 22, 0, 0))
                };
            }

            public IEnumerable<MusicEvent> GetTickets(DateTime date)
            {
                return this._musicEvents.Where(ticket => ticket.Start.Date == date.Date);
            }
        }

        public class MusicEvent
        {

            public MusicEvent(string band, DateTime start, DateTime end)
            {
                this.Band = band;
                this.Start = start;
                this.End = end;
            }

            public string Band { get; }
            public DateTime Start { get; }
            public DateTime End { get; }
        }

        #endregion
    }
}
