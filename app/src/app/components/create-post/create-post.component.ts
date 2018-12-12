import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { CreatePost } from '../../model';
import { CreatePostService, SnackbarService } from '../../services';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent implements OnInit {

  @ViewChild('fileInput') private fileInput: ElementRef;
  private internalIsLoading = false;
  private internalPostText = '';

  constructor(
    public dialogRef: MatDialogRef<CreatePostComponent>,
    private createPostService: CreatePostService,
    private snackbarService: SnackbarService) {
  }

  public get isLoading(): boolean {
    return this.internalIsLoading;
  }

  public set isLoading(value: boolean) {
    this.internalIsLoading = value;
  }

  public set postText(value: string) {
    this.internalPostText = value;
  }

  public get postText(): string {
    return this.internalPostText;
  }

  public get otherFileName(): string {
    const file = this.getFile();
    return file ? file.name : 'No file selected';
  }

  public ngOnInit(): void {
  }

  public onNoClick(): void {
    this.dialogRef.close(-1);
  }

  public closeDialog(): void {
    this.dialogRef.close(-1);
  }

  public selectFile(): void {
    this.fileInput.nativeElement.click();
  }

  public async createPost(): Promise<void> {
    if (!this.getFile()) {
      this.snackbarService.showMessage('No file selected');
      return;
    }

    try {
      this.isLoading = true;
      const createPost = await this.createCreatePostModel(this.getFile());
      const result = await this.createPostService.createPost(createPost);
      if (result.success) {
        this.dialogRef.close(result);
      }
    } finally {
      this.isLoading = false;
    }
  }

  public createCreatePostModel(file: any): CreatePost {
    return new CreatePost(this.postText, file);
  }

  private getFile(): any {
    if (this.fileInput.nativeElement.files && this.fileInput.nativeElement.files[0]) {
      return this.fileInput.nativeElement.files[0];
    }

    return null;
  }
}
