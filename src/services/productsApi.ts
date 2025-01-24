import axios from 'axios';
import { Product } from '../types/product';

const BASE_URL = 'https://www.fioriforyou.com/backfiori';

interface ApiResponse {
  status: string;
  count: number;
  products: {
    id_product: string;
    reference_product: string;
    nom_product: string;
    img_product: string;
    img2_product?: string;
    img3_product?: string;
    img4_product?: string;
    description_product: string;
    type_product: string;
    category_product: string;
    itemgroup_product: string;
    price_product: string;
    qnty_product: string;
    "3xl_size": string;
    "4xl_size": string;
    xs_size: string;
    s_size: string;
    m_size: string;
    l_size: string;
    xl_size: string;
    xxl_size: string;
    "48_size": string;
    "50_size": string;
    "52_size": string;
    "54_size": string;
    "56_size": string;
    "58_size": string;
    status_product: string;
    discount_product: string;
    related_products: string;
    color_product: string;
    createdate_product: string;
  }[];
}

interface SingleProductResponse {
  status: string;
  product: ApiResponse['products'][0];
}

const transformProductData = (productData: ApiResponse['products'][0]): Product => ({
  id: parseInt(productData.id_product),
  name: productData.nom_product,
  material: productData.type_product,
  color: productData.color_product,
  price: parseFloat(productData.price_product) || 0.0,
  image: `${BASE_URL}/${productData.img_product}?format=webp&quality=70`,
  image2: productData.img2_product ? `${BASE_URL}/${productData.img2_product}?format=webp&quality=70` : undefined,
  image3: productData.img3_product ? `${BASE_URL}/${productData.img3_product}?format=webp&quality=70` : undefined,
  image4: productData.img4_product ? `${BASE_URL}/${productData.img4_product}?format=webp&quality=70` : undefined,
  description: productData.description_product,
  status: productData.status_product,
  reference: productData.reference_product,
  itemGroup: productData.itemgroup_product,
  relatedProducts: productData.related_products,
  colorProduct: productData.color_product,
  discount_product: productData.discount_product || "",
  sizes: {
    xs: parseInt(productData.xs_size) || 0,
    s: parseInt(productData.s_size) || 0,
    m: parseInt(productData.m_size) || 0,
    l: parseInt(productData.l_size) || 0,
    xl: parseInt(productData.xl_size) || 0,
    xxl: parseInt(productData.xxl_size) || 0,
    "4xl": parseInt(productData["4xl_size"]) || 0,
    "3xl": parseInt(productData["3xl_size"]) || 0,
    "48": parseInt(productData["48_size"]) || 0,
    "50": parseInt(productData["50_size"]) || 0,
    "52": parseInt(productData["52_size"]) || 0,
    "54": parseInt(productData["54_size"]) || 0,
    "56": parseInt(productData["56_size"]) || 0,
    "58": parseInt(productData["58_size"]) || 0,
  },
  quantity: parseInt(productData.qnty_product) || 0,
  type_product: productData.type_product,
  category_product: productData.category_product,
  itemgroup_product: productData.itemgroup_product,
});

export const fetchAllProducts = async (): Promise<Product[]> => {
  try {
    const timestamp = new Date().getTime(); // Add timestamp to prevent caching
    const response = await axios.get<ApiResponse>(`${BASE_URL}/get_all_articles.php?timestamp=${timestamp}`);

    if (response.data.status === 'success') {
      return response.data.products
        .filter(product => product.qnty_product !== "0" && parseInt(product.qnty_product) > 0)
        .map(transformProductData);
    }

    throw new Error(`Failed to fetch products: ${response.data.status}`);
  } catch (error) {
    console.error('Error fetching products:', error);
    throw error;
  }
};

export const fetchSingleProduct = async (productId: number): Promise<Product> => {
  try {
    const timestamp = new Date().getTime(); // Add timestamp to prevent caching
    const response = await axios.get<SingleProductResponse>(
      `${BASE_URL}/get_single_product.php?id_product=${productId}&timestamp=${timestamp}`
    );

    if (response.data.status === 'success' && response.data.product) {
      return transformProductData(response.data.product);
    }

    throw new Error(`Failed to fetch product: ${response.data.status}`);
  } catch (error) {
    console.error('Error fetching single product:', error);
    throw error;
  }
};