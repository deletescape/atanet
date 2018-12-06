import { Component, OnInit, Inject, EventEmitter, Output, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { CreatePost, File } from '../../model';
import { CreatePostService, SnackbarService, LocationService, FileService } from '../../services';
import { CreateFileComponent } from '../create-file';
import { FileDialogComponent } from '../file-dialog';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent {

  @ViewChild('fileInput') private fileInput: ElementRef;
  private internalIsLoading = false;
  private internalPostText = '';
  private selectedFile: number | undefined = undefined;

  @Output() private postCreated = new EventEmitter<number>();

  constructor(
    public dialogRef: MatDialogRef<CreatePostComponent>,
    private fileService: FileService,
    private createPostService: CreatePostService,
    private snackbarService: SnackbarService,
    private locationService: LocationService,
    private dialog: MatDialog) {
  }

  public get fileId(): number {
    return this.selectedFile;
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
    if (this.fileInput.nativeElement.files && this.fileInput.nativeElement.files[0]) {
      return this.fileInput.nativeElement.files[0].name;
    } else {
      return 'No file selected';
    }
  }

  public onFilePreview(): void {
    if (this.selectedFile) {
      this.isLoading = true;
      this.fileService.getFile(this.selectedFile).then(x => {
        this.isLoading = false;
        this.dialog.open(FileDialogComponent, {
          width: '70%',
          data: {
            file: <File>x
          }
        });
      });
    }
  }

  public onNoClick(): void {
    this.dialogRef.close(-1);
  }

  public closeDialog(): void {
    this.dialogRef.close(-1);
  }

  public removeFile(): void {
    this.selectedFile = undefined;
  }

  public selectFile(): void {
    this.fileInput.nativeElement.click();
  }

  public async createFile(): Promise<void> {
    if (this.fileInput.nativeElement.files && this.fileInput.nativeElement.files[0]) {
      const result = await this.fileService.addFile(this.fileInput.nativeElement.files[0]);
      if (result !== undefined) {
        this.selectedFile = result.createdId;
        this.onFilePreview();
      } else {
        const message = 'Something went wrong. The file may be too large';
        this.snackbarService.showMessage(message);
        throw new Error(message);
      }
    } else {
      throw new Error('No file selected');
    }
  }

  public async createPost(): Promise<void> {
    try {
      this.isLoading = true;
      const createPost = await this.createCreatePostModel();
      await this.createFile();
      if (this.selectedFile) {
        createPost.fileId = this.selectedFile;
      }
      const result = await this.createPostService.createPost(createPost);
      this.dialogRef.close(result);
      this.snackbarService.showMessage('Post created');
      this.postCreated.emit(result);
    } finally {
      this.isLoading = false;
    }
  }

  public async createCreatePostModel(): Promise<CreatePost> {
    const location = await this.locationService.getLocation();
    return new CreatePost(this.postText, <number>location['latitude'], <number>location['longitude']);
  }
}
