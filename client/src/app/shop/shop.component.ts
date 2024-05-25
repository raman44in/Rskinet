import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ShopService } from './shop.service';
import { Product } from '../shared/models/product.ts';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  shopParams=new ShopParams();
  sortOptions=[
    {name:'Alphabetical',value:'name'},
    {name:'Price Low to High',value:'priceAsc'},
    {name:'Price High to Low',value:'priceDesc'}
  ];
sortSelected:string='name';
totalCount:number=0;
@ViewChild('search') searchTerm?:ElementRef
  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        this.products = response.data;
        this.shopParams.pageNumber=response.pageIndex;
        this.shopParams.pageSize=response.pageSize;
        this.totalCount=response.count;
        console.log(response);
      },
      error: (error) => console.log(error),
    });
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => {
        this.brands = [{id:0,name:'All'},...response];
        console.log(response);
      },
      error: (error) => console.log(error),
    });
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) => {
        this.types = [{id:0,name:'All'},...response];
        console.log(response);
      },
      error: (error) => console.log(error),
    });
  }

  onBrandSelected(brandId:number){
    this.shopParams.brandId=brandId;
    this.shopParams.pageNumber=1;
    this.getProducts();

  }

  onTypeSelected(typeId:number){
    this.shopParams.typeId=typeId;
    this.getProducts();

  }

  onSortSelected(event:any){
    this.shopParams.sort=event.target.value;
    this.shopParams.pageNumber=1;
    this.getProducts();

  }
  onPageChanged(event:any){
    if(this.shopParams.pageNumber!=event.page){
      this.shopParams.pageNumber=event.page
      this.getProducts();
    }

  }

  onSearch(){
    this.shopParams.search=this.searchTerm?.nativeElement.value;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }

  onReset(){
    if(this.searchTerm) this.searchTerm.nativeElement.value='';
    this.shopParams=new ShopParams();
    this.getProducts();
  }


}

