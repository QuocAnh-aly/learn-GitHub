import { useState, useEffect } from 'react';

function App() { 
  const [products, setProducts] = useState([]);

  useEffect(() => {
    const mockData = [
      { id: 1, name: 'Laptop Dell', soLuong: 10, noiSX: 'Mỹ' },
      { id: 2, name: 'Chuột Logitech', soLuong: 50, noiSX: 'Trung Quốc' },
      { id: 3, name: 'Bàn phím Keychron', soLuong: 25, noiSX: 'Hồng Kông' },
    ];
    setProducts(mockData);
    
  }, []);
  return (
    <div>
      <h1>Product List</h1>
      <table border="1">
        <thead>
          <tr>
            <th>ID</th>
            <th>Ten</th>
            <th>So Luong</th>
            <th>Noi SX</th>
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.name}</td>
              <td>{p.soLuong}</td>
              <td>{p.noiSX}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default App;