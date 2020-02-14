import React, { Component } from "react";

class ExistingOrder extends Component {
  render() {
    const { order, onOrderRemove } = this.props;
    console.log(order);
    return (
      <div>
        <span>Order ID: {order.id}</span>
        <button onClick={() => onOrderRemove(order.id)}>Cancel</button>
        {order.foods.map((food, index) => {
          return <li key={index}>Food ID: {food}</li>;
        })}
      </div>
    );
  }
}

export default ExistingOrder;
