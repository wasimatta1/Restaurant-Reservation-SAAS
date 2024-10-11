﻿using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces.BaseInterface;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Task<IEnumerable<Reservation>> GetReservationsByCustomer(int CustomerId);
    }
}
