namespace ThAmCo.Events.Data
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }
    }
}
