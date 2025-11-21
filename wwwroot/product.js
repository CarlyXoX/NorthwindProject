document.addEventListener("DOMContentLoaded", function () {
    fetchProducts();
});
document.getElementById("CategoryId").addEventListener("change", (e) => {
    document.getElementById('product_rows').dataset['id'] = e.target.value;
    fetchProducts();
});
document.getElementById('Discontinued').addEventListener("change", (e) => {
    fetchProducts();
});
async function fetchProducts() {
    const id = document.getElementById('product_rows').dataset['id'];
    const discontinued = document.getElementById('Discontinued').checked ? "" : "/discontinued/false";
    const { data: fetchedProducts } = await axios.get(`../../api/category/${id}/product${discontinued}`);
    // console.log(fetchedProducts);
    let product_rows = "";
    fetchedProducts.map(product => {
        const css = product.discontinued ? " class='discontinued'" : "";
        console.log(css);
        product_rows +=
            `<tr${css} data-id="${product.productId}">
        <td>${product.productName}</td>
        <td class="text-end">${product.unitPrice.toFixed(2)}</td>
        <td class="text-end">${product.unitsInStock}</td>
      </tr>`;
    });
    document.getElementById('product_rows').innerHTML = product_rows;
}