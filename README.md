# Monaco Grand Prix Ticket Reservation API
# Use cases
The ticket reservation system for the event "F1 Grand Prix De Monaco 2024" should enable potential customers to reserve tickets for this event. On the homepage, users can access basic information such as the race date and the participating drivers. Users can view ticket prices for specific zones and days of the race, using applied filters. In real-time, users can see the total reservation cost for the selected days and seating zones. If they qualify for the early bird offer, they will receive an additional discount. Furthermore, selecting multiple days will grant another discount. Finally, users can also apply a promo code from a friend who has already made a reservation to receive a third type of discount. After selecting the reservation details, users are required to provide personal information. A user can reserve a ticket and, in doing so, receive a unique token that can later be used to access, modify, or cancel their reservation.

# Technologies
The ticket reservation system for the "F1 Grand Prix De Monaco 2024" event is developed using ASP.NET Core for the backend and Entity Framework Core for database operations. The system uses a relational database with models and migrations handled via Entity Framework. The frontend is built with React, and Axios is used for connecting the frontend to the backend via RESTful APIs.
All validations are implemented on both the frontend and backend to ensure security and reduce unnecessary requests to the server.

Relational Model:
1. RaceDay(RaceDayID,Date)
2. SeatingZone(ZoneID,Name,Capacity,Price,IsAccessible,HasLargeTV)
3. BookingDayZones(BookingID,ZoneID,RaceDayID)
4. Booking(BookingID,Token,PromoCodeUsed,BookingStatus,OriginalPrice,
FinalPrice, IsEarlBird, CustomerID)
5. Customer(CustomerID, FirstName, LastName, Company, Address1,
Address2, PostalCode, City, Country, Email)
6. PromoCode(PromoCodeID,Code,IsUsed,Bookingld,UsedByBookingld)
