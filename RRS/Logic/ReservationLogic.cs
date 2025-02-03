public static class ReservationLogic {
    public static string RetrieveReservations(int restaurantID) 
    {
        string returnResult = "";
        int i = 0;

        //grab reserv list
        List<Reservations> reservations = Database.SelectReservations(restaurantID);
        if (reservations.Count > 0) {
            foreach (Reservations reservation in reservations) 
            {
                i++;
                //grab reservation timeslot
                ReservationTimeSlots reservationTimeSlot = Database.SelectReservationTimeSlot(reservation.TimeSlotID);
            
                if (reservationTimeSlot.EndDateTime >= DateTime.Now) {
                    //retrieve acc info
                    Accounts accounts = Database.SelectAccount(reservation.AccountID);
                    
                    //add enter to every line
                    if (returnResult != "") 
                    {
                        returnResult += "\n";
                    }

                    if (reservation.Status == 1) {
                        returnResult += $"Reservation for: {accounts.FirstName} {accounts.LastName} {accounts.Age} {accounts.Gender}, Table: {reservation.TableID}, Date: {reservationTimeSlot.GetDate()}, from {reservationTimeSlot.GetStartTime24()} to {reservationTimeSlot.GetEndTime24()} - CANCELLED";
                    } else {
                        returnResult += $"Reservation for: {accounts.FirstName} {accounts.LastName} {accounts.Age} {accounts.Gender}, Table: {reservation.TableID}, Date: {reservationTimeSlot.GetDate()}, from {reservationTimeSlot.GetStartTime24()} to {reservationTimeSlot.GetEndTime24()}";
                    }
                }
            }
        } else {
            returnResult = "No reservations found";
        }
        
        return returnResult;
    }

    public static List<Reservations> RetrieveCurrReservations(int restaurantID) {
        List<Reservations> reservations = Database.SelectReservations(restaurantID);

        IEnumerable<Reservations> returnValue =
            from Reservations in reservations
            where TimeSlotLogic.GetSelectedTimeSlot(Reservations.TimeSlotID).StartDateTime > DateTime.Now
            select Reservations;

        return returnValue.ToList();
    }

    public static List<Reservations> RemoveCancelled(List<Reservations> reservations) {
        IEnumerable<Reservations> returnValue =
            from Reservations in reservations
            where Reservations.Status != 1
            select Reservations;

        return returnValue.ToList();
    }

    public static string RetrieveCurrentReservations(int restaurantID) 
    {
        string returnResult = "";
        int i = 0;

        //grab reserv list
        List<Reservations> reservations = Database.SelectReservations(restaurantID);
        if (reservations.Count > 0) {
            foreach (Reservations reservation in reservations) 
            {
                i++;
                //grab reservation timeslot
                ReservationTimeSlots reservationTimeSlot = Database.SelectReservationTimeSlot(reservation.TimeSlotID);
                if (reservationTimeSlot.StartDateTime <= DateTime.Now & reservationTimeSlot.EndDateTime >= DateTime.Now) {
                    //retrieve acc info
                    Accounts accounts = Database.SelectAccount(reservation.AccountID);
                    
                    //add enter to every line
                    if (returnResult != "") 
                    {
                        returnResult += "\n";
                    }

                    //format return
                    if (reservation.Status == 1) {
                        returnResult += $"Reservation for: {accounts.FirstName} {accounts.LastName} ({accounts.Age}) {accounts.Gender}, Table: {reservation.TableID}, Date: {reservationTimeSlot.GetDate()}, from {reservationTimeSlot.GetStartTime24()} to {reservationTimeSlot.GetEndTime24()} - CANCELLED";
                    } else {
                        returnResult += $"Reservation for: {accounts.FirstName} {accounts.LastName} ({accounts.Age}) {accounts.Gender}, Table: {reservation.TableID}, Date: {reservationTimeSlot.GetDate()}, from {reservationTimeSlot.GetStartTime24()} to {reservationTimeSlot.GetEndTime24()}";
                    }
                }
            }
        } else {
            returnResult = "No reservations found";
        }

        return returnResult;
    }

    public static List<Reservations> RetrievePreviousReservations(int restaurantID, int AccountID) 
    {
        List<Reservations> returnResult = [];
        int i = 0;

        //grab reserv list
        List<Reservations> reservations = Database.SelectReservations(restaurantID, AccountID);
        
        if (reservations.Count > 0) {
            foreach (Reservations reservation in reservations) 
            {
                i++;
                //grab reservation timeslot
                ReservationTimeSlots reservationTimeSlot = Database.SelectReservationTimeSlot(reservation.TimeSlotID);
                if (reservationTimeSlot.StartDateTime <= DateTime.Now) {
                    returnResult.Add(reservation);
                }
            }
        }
        
        return returnResult;
    }

 public static string RetrieveReservations(int restaurantID, int AccountID) {
    string returnResult = "";
    int i = 0;

    List<Reservations> reservations = Database.SelectReservations(restaurantID, AccountID);
    
    if (reservations.Count == 0) {
     return "No reservations found.";}


    foreach (Reservations reservation in reservations) {
        i++;

        ReservationTimeSlots reservationTimeSlot = Database.SelectReservationTimeSlot(reservation.TimeSlotID);


        if (reservationTimeSlot.EndDateTime >= DateTime.Now) {
            Accounts accounts = Database.SelectAccount(reservation.AccountID);

            List<Reservations> TableReservations = Database.SelectReservationsForTableAndTimeSlot(reservation.TimeSlotID, reservation.TableID);




            string matchmakingInfo = "";
    

    foreach (Reservations tableReservation in TableReservations) {
    if (tableReservation == null) {
        Console.WriteLine("tableReservation is null!");
    } 
    
    if (tableReservation.AccountID != reservation.AccountID) {
        Accounts partnerAccount = Database.SelectAccount(tableReservation.AccountID);
        matchmakingInfo += $"{partnerAccount.Gender} ({partnerAccount.Age}), ";
    }

            if (returnResult != "") {
                returnResult += "\n";
            }

            returnResult += $"{i} - Reservation: {reservation.ID}, Table: {reservation.TableID}, Matchmaking Partners: {(!string.IsNullOrEmpty(matchmakingInfo) ? matchmakingInfo.TrimEnd(',', ' ') : "None")}, Date: {reservationTimeSlot.GetDate()}, from {reservationTimeSlot.GetStartTime24()} to {reservationTimeSlot.GetEndTime24()}";
        }}}
    

    return returnResult;
}


    public static List<Reservations> RetrieveReservations(int restaurantID, Reservations reservations) => Database.SelectReservations(restaurantID, reservations);

    public static bool CreateReservation(int restaurantID, int TimeSlotID, int AccountID, int table) {
        return Database.Insert(new Reservations(restaurantID, TimeSlotID, table, AccountID, 0));
    }

    public static bool CancelReservationsBulk(int timeslotID, Accounts LoggedAccount) => Database.UpdateReservationStatusBulk(timeslotID, 1);

    public static bool CancelReservation(Reservations reservations) => Database.UpdateReservationStatus(reservations, 1);

    public static List<string> ConvertToDisplayString(List<Reservations> reservations) {
        List<string> returnValue = [];
        foreach (Reservations reservation in reservations) {
            ReservationTimeSlots reservationTimeSlot = Database.SelectReservationTimeSlot(reservation.TimeSlotID);
            returnValue.Add($"Reservation: {reservation.ID}, Table: {reservation.TableID}, Date: {reservationTimeSlot.GetDate()}, from {reservationTimeSlot.GetStartTime24()} to {reservationTimeSlot.GetEndTime24()}");
        }
        return returnValue;
    }

   

}