import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule, TabsModule, BsDatepickerModule, PaginationModule, ButtonsModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';


import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { RecipesListComponent } from './recipes/recipes-list/recipes-list.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-list/member-card/member-card.component';
import { RecipesCardComponent } from './recipes/recipes-list/recipes-card/recipes-card.component';
import { MemberDetailComponent } from './members/member-list/member-detail/member-detail.component';
import { RecipeDetailComponent } from './recipes/recipes-list/recipe-detail/recipe-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail-resolver';
import { RecipeDetailResolver } from './_resolvers/recipe-detail-resolver';
import { MemberListResolver } from './_resolvers/member-list-resolver';
import { RecipeListResolver } from './_resolvers/recipe-list-resolver';
import { MemberEditComponent } from './members/member-list/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit-resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './members/member-list/photo-editor/photo-editor.component';
import { MemberRecipeCardComponent } from './members/member-list/member-recipe-card/member-recipe-card.component';
import { AddRecipeComponent } from './recipes/recipes-list/add-recipe/add-recipe.component';
import { RecipeEditorComponent } from './members/member-list/recipe-editor/recipe-editor.component';
import { RecipeEditComponent } from './recipes/recipes-list/recipe-edit/recipe-edit.component';
import { RecipeEditResolver } from './_resolvers/recipe-edit-resolver';
import {TimeAgoPipe} from 'time-ago-pipe';
import { ListsResolver } from './_resolvers/lists.resovler';
import { LikeListsComponent } from './lists/like-lists/like-lists.component';
import { FavRecipeListsComponent } from './lists/like-lists/favRecipe-lists/favRecipe-lists.component';
import { FavRecipesResolver } from './_resolvers/fav-recipes-resolver';
import { FavRecipeCardComponent } from './lists/like-lists/favRecipe-lists/fav-recipe-card/fav-recipe-card.component';
import { MessagesResolver } from './_models/messages-resolver';
import { MemberMessagesComponent } from './members/member-list/member-messages/member-messages.component';
import { FooterComponent } from './footer/footer.component';
import { RecipePrintComponent } from './recipes/recipes-list/recipe-print/recipe-print.component';




export function tokenGetter() {
   return localStorage.getItem('token');
}

export class CustomHammerConfig extends HammerGestureConfig  {
   overrides = {
       pinch: { enable: false },
       rotate: { enable: false }
   };
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      RecipesListComponent,
      MemberListComponent,
      LikeListsComponent,
      FavRecipeListsComponent,
      MessagesComponent,
      MemberCardComponent,
      RecipesCardComponent,
      MemberDetailComponent,
      FavRecipeCardComponent,
      RecipeDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      MemberRecipeCardComponent,
      AddRecipeComponent,
      RecipeEditorComponent,
      MemberMessagesComponent,
      RecipeEditComponent,
      TimeAgoPipe,
      FooterComponent,
      RecipePrintComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      NgxGalleryModule,
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      FileUploadModule,
      ReactiveFormsModule,
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      MemberDetailResolver,
      RecipeDetailResolver,
      MemberListResolver,
      RecipeListResolver,
      MessagesResolver,
      MemberEditResolver,
      FavRecipesResolver,
      RecipeEditResolver,
      ListsResolver,
      PreventUnsavedChanges,
      { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
