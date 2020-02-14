import React, { Component } from "react";
import Order from "./order";
import Select from "react-select";
import ExistingOrder from "./existingOrder";

class Orders extends Component {
  state = {
    locations: [],
    selectedOption: null,
    databaseUser: {},
    existingOrders: []
  };

  componentDidMount() {
    console.log("loading user");
    ////////////////
    // here should be taken googleUserId of the logged in user, and not hard coded!!!!
    // currently we do not have login on front end
    ///////////////
    fetch(
      "http://localhost:6879/api/customer/Query?googleUserId=aberonja@execom.eu&Page=1&Size=1"
    )
      .then(response => response.json())
      .then(response => {
        console.log(response);
        this.setState({ databaseUser: response.items[0] });
        console.log("user is loaded");

        this.getExistingOrders();
      })
      .catch(err => console.log(err));

    fetch("http://localhost:6879/api/location/Query?Page=1&Size=100")
      .then(response => response.json())
      .then(response => {
        this.setState({
          locations: response.items.map(item => {
            return { label: item.name, value: item.id };
          })
        });
      })
      .catch(err => console.log(err));
  }

  handleLocationChange = selectedOption => {
    this.setState({ selectedOption });
    const { databaseUser } = this.state;

    const url = "http://localhost:6879/api/Customer/" + databaseUser.id;
    fetch(url, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        withCredentials: true
      },
      body: JSON.stringify({
        name: databaseUser.name,
        balance: databaseUser.balance,
        locationId: selectedOption.value,
        googleUserId: databaseUser.googleUserId,
        roleId: databaseUser.roleId
      })
    })
      .then(response => {
        console.log(response);
      })
      .catch(error => {
        console.log("check in error", error);
      });
  };

  handleSubmit = () => {
    fetch("http://localhost:6879/api/meal/", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        price: this.getOrderPrice(),
        foods: this.props.orders.map(food => {
          return food.id;
        })
      })
    })
      .then(response => response.json())
      .then(data => {
        fetch("http://localhost:6879/api/orders/", {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            price: this.getOrderPrice(),
            date: new Date().toISOString(),
            mealId: data.id,
            customerId: this.state.databaseUser.id,
            note: "none"
          })
        })
          .then(response => response.json())
          .then(response => {
            if (response.id !== 0) {
              this.props.onOrderSubmitted();
              this.getExistingOrders();
            }
          })
          .catch(error => console.log(error));
      })
      .catch(error => console.log(error));
  };

  getOrderPrice = () => {
    const { orders } = this.props;
    return orders.reduce((total, food) => total + food.price, 0);
  };

  getExistingOrders = () => {
    const url =
      "http://localhost:6879/api/orders/Query?Date=" +
      new Date().toISOString() +
      "&CustomerId=" +
      this.state.databaseUser.id +
      "&Page=1&Size=100";
    fetch(url)
      .then(response => response.json())
      .then(response => {
        this.setState({ existingOrders: [] });
        response.items.map(meal => {
          fetch(
            "http://localhost:6879/api/meal/Query?Id=" +
              meal.mealId +
              "&Page=1&Size=100"
          )
            .then(response => response.json())
            .then(response => {
              this.setState({
                existingOrders: [
                  ...this.state.existingOrders,
                  response.items[0]
                ]
              });
            })
            .catch(err => console.log(err));
        });
      })
      .catch(err => console.log(err));
  };

  handleOrderRemove = orderId => {
    console.log("handleOrderRemove called");
    fetch("http://localhost:6879/api/meal/" + orderId, {
      method: "DELETE",
      headers: {
        Accept: "application/json"
      }
    })
      .then(response => {
        if (response.status === 204) {
          this.setState({
            existingOrders: this.state.existingOrders.filter(
              x => x.id !== orderId
            )
          });
        }
      })
      .catch(error => console.log(error));
  };
  render() {
    const { orders, onRemoveFromOrder } = this.props;
    const { locations, existingOrders } = this.state;
    return (
      <div>
        <h2>Order</h2>
        <p>Location for delivery:</p>
        <Select options={locations} onChange={this.handleLocationChange} />
        <h3>Added meals:</h3>
        {orders.map((order, index) => {
          return (
            <Order
              key={index}
              order={order}
              onRemoveFromOrder={onRemoveFromOrder}
            />
          );
        })}
        <h3>Order price: {this.getOrderPrice()}</h3>

        <button disabled={orders.length === 0} onClick={this.handleSubmit}>
          Submit order
        </button>

        <h3>Existing order(s):</h3>
        {existingOrders.map((order, index) => {
          return (
            <ExistingOrder
              key={index}
              order={order}
              onOrderRemove={this.handleOrderRemove}
            />
          );
        })}
      </div>
    );
  }
}

export default Orders;
